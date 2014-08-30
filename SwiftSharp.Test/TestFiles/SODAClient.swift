//
//  SODAClient.swift
//  SODAKit
//
//  Created by Frank A. Krueger on 8/9/14.
//  Copyright (c) 2014 Socrata, Inc. All rights reserved.
//

import Foundation

// Reference: http://dev.socrata.com/consumers/getting-started.html

/// The default number of items to return in SODAClient.queryDataset calls.
let SODADefaultLimit = 1000

/// The result of an asynchronous SODAClient.queryDataset call. It can either succeed with data or fail with an error.
enum SODADatasetResult {
    case Dataset ([[String: AnyObject]])
    case Error (NSError)
}

/// The result of an asynchronous SODAClient.getRow call. It can either succeed with data or fail with an error.
enum SODARowResult {
    case Row ([String: AnyObject])
    case Error (NSError)
}

/// Callback for asynchronous queryDataset methods of SODAClient
typealias SODADatasetCompletionHandler = SODADatasetResult -> Void

/// Callback for asynchronous getRow method of SODAClient
typealias SODARowCompletionHandler = SODARowResult -> Void

/// Consumes data from a Socrata OpenData end point.
class SODAClient {

    let domain: String
    let token: String

    /// Initializes this client to communicate with a SODA endpoint.
    init(domain: String, token: String) {
        self.domain = domain
        self.token = token
    }

    /// Gets a row using its identifier. See http://dev.socrata.com/docs/row-identifiers.html
    func getRow(row: String, inDataset: String, _ completionHandler: SODARowCompletionHandler) {
        getDataset("\(inDataset)/\(row)", withParameters: [:]) { res in
            switch res {
            case .Dataset (let rows):
                completionHandler(.Row (rows[0]))
            case .Error(let err):
                completionHandler(.Error (err))
            }
        }
    }

    /// Asynchronously gets a dataset using a simple filter query. See http://dev.socrata.com/docs/filtering.html
    func getDataset(dataset: String, withFilters: [String: String], limit: Int = SODADefaultLimit, offset: Int = 0, _ completionHandler: SODADatasetCompletionHandler) {
        var ps = withFilters
        ps["$limit"] = "\(limit)"
        ps["$offset"] = "\(offset)"
        getDataset(dataset, withParameters: ps, completionHandler)
    }

    /// Low-level access for asynchronously getting a dataset. You should use SODAQueries instead of this. See http://dev.socrata.com/docs/queries.html
    func getDataset(dataset: String, withParameters: [String: String], _ completionHandler: SODADatasetCompletionHandler) {
        // Get the URL
        let query = SODAClient.paramsToQueryString (withParameters)
        let path = dataset.hasPrefix("/") ? dataset : ("/resource/" + dataset)
        
        let url = "https://\(self.domain)\(path).json?\(query)"
        let urlToSend = NSURL(string: url)
        
        // Build the request
        let request = NSMutableURLRequest(URL: urlToSend)
        request.setValue("application/json", forHTTPHeaderField:"Accept")
        request.setValue(self.token, forHTTPHeaderField:"X-App-Token")
        
        // Send it
        let session = NSURLSession.sharedSession()
        var task = session.dataTaskWithRequest(request, completionHandler: { data, response, reqError in
            
            // We sync the callback with the main thread to make UI programming easier
            let syncCompletion = { res in NSOperationQueue.mainQueue().addOperationWithBlock { completionHandler (res) } }
            
            // Give up if there was a net error
            if reqError != nil {
                syncCompletion(.Error (reqError))
                return
            }
            
            // Try to parse the JSON
//            println(NSString (data: data, encoding: NSUTF8StringEncoding))
            
            var jsonError: NSError?
            var jsonResult: AnyObject! = NSJSONSerialization.JSONObjectWithData(data, options: NSJSONReadingOptions.MutableContainers, error: &jsonError)
            if let error = jsonError {
                syncCompletion(.Error (error))
                return
            }
            
            // Interpret the JSON
            if let a = jsonResult as? [[String: AnyObject]] {
                syncCompletion(.Dataset (a))
            }
            else if let d = jsonResult as? [String: AnyObject] {
                if let e : AnyObject = d["error"] {
                    if let m : AnyObject = d["message"] {
                        syncCompletion(.Error (NSError(domain: "SODA", code: 0, userInfo: ["Error": m])))
                        return
                    }
                }
                syncCompletion(.Dataset ([d]))
            }
            else {
                syncCompletion(.Error (NSError()))
            }
        })
        task.resume()
    }
    
    /// Converts an NSDictionary into a query string.
    private class func paramsToQueryString (params: [String: String]) -> String {
        var s = ""
        var head = ""
        for (key, value) in params {
            let sk = key.stringByAddingPercentEscapesUsingEncoding(NSUTF8StringEncoding)
            let sv = value.stringByAddingPercentEscapesUsingEncoding(NSUTF8StringEncoding)
            s += "\(head)\(sk)=\(sv)"
            head = "&"
        }
        return s
    }
}

/// SODAQuery extension to SODAClient
extension SODAClient {
    /// Get a query object that can be used to query the client using a fluent syntax.
    func queryDataset(dataset: String) -> SODAQuery {
        return SODAQuery (client: self, dataset: dataset)
    }
}

/// Assists in the construction of a SoQL query.
class SODAQuery
{
    let client: SODAClient
    let dataset: String
    
    let parameters: [String: String]
    
    /// Initializes all the parameters of the query
    init(client: SODAClient, dataset: String, parameters: [String: String] = [:])
    {
        self.client = client
        self.dataset = dataset
        self.parameters = parameters
    }

    /// Generates SoQL $select parameter. Use the AS operator to modify the output.
    func select(select: String) -> SODAQuery {
        var ps = self.parameters
        ps["$select"] = select
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates SoQL $where parameter. Use comparison operators and AND, OR, NOT, IS NULL, IS NOT NULL. Strings must be single-quoted.
    func filter(filter: String) -> SODAQuery {
        var ps = self.parameters
        ps["$where"] = filter
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates simple filter parameter. Multiple filterColumns are allowed in a single query.
    func filterColumn(column: String, _ value: String) -> SODAQuery {
        var ps = self.parameters
        ps[column] = value
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }

    /// Generates SoQL $q parameter. This uses a multi-column full text search.
    func fullText(fullText: String) -> SODAQuery {
        var ps = self.parameters
        ps["$q"] = fullText
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates SoQL $order ASC parameter.
    func orderAscending(column: String) -> SODAQuery {
        var ps = self.parameters
        ps["$order"] = "\(column) ASC"
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }

    /// Generates SoQL $order DESC parameter.
    func orderDescending(column: String) -> SODAQuery {
        var ps = self.parameters
        ps["$order"] = "\(column) DESC"
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates SoQL $group parameter. Use select() with aggregation functions like MAX and then name the column to group by.
    func group(column: String) -> SODAQuery {
        var ps = self.parameters
        ps["$group"] = column
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates SoQL $limit parameter. The default limit is 1000.
    func limit(limit: Int) -> SODAQuery {
        var ps = self.parameters
        ps["$limit"] = "\(limit)"
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Generates SoQL $offset parameter.
    func offset(offset: Int) -> SODAQuery {
        var ps = self.parameters
        ps["$offset"] = "\(offset)"
        return SODAQuery (client: self.client, dataset: self.dataset, parameters: ps)
    }
    
    /// Performs the query asynchronously and sends all the results to the completion handler.
    func get(completionHandler: SODADatasetResult -> Void) {
        client.getDataset(dataset, withParameters: parameters, completionHandler)
    }

    /// Performs the query asynchronously and sends the results, one row at a time, to an iterator function.
    func each(iterator: SODARowResult -> Void) {
        client.getDataset(dataset, withParameters: parameters) { res in
            switch res {
            case .Dataset (let data):
                for row in data {
                    iterator(.Row (row))
                }
            case .Error (let err):
                iterator(.Error (err))
            }
        }
    }
}

