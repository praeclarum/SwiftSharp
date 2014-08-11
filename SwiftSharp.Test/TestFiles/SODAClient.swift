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

/// Enabled consuming data from a Socrata end point.
class SODAClient {

    let domain: String
    let token: String

    /// Initializes this client to communicate with a SODA endpoint.
    init(domain: String, token: String) {
        self.domain = domain
        self.token = token
    }

    /// Gets a row using its identifier. See http://dev.socrata.com/docs/row-identifiers.html
    func getRow(row: String, inDataset: String, completionHandler: SODARowCompletionHandler) {
        let ps = NSMutableDictionary()
        getResource("\(inDataset)/\(row)", params: ps, completionHandler: { res in
            switch res {
            case .Dataset (let rows):
                completionHandler(.Row (rows[0]))
            case .Error(let err):
                completionHandler(.Error (err))
            }})
    }

    /// Queries an entire dataset. See http://dev.socrata.com/docs/filtering.html
    func queryDataset(dataset: String, limit: Int = SODADefaultLimit, offset: Int = 0, completionHandler: SODADatasetCompletionHandler) {
        let ps = NSMutableDictionary()
        ps["$limit"] = "\(limit)"
        ps["$offset"] = "\(offset)"
        getResource(dataset, params: ps, completionHandler: completionHandler)
    }
    
    /// Queries a dataset using simple filters. See http://dev.socrata.com/docs/filtering.html
    func queryDataset(dataset: String, withFilters: [String: String], limit: Int = SODADefaultLimit, offset: Int = 0, completionHandler: SODADatasetCompletionHandler) {
        let ps = NSMutableDictionary(dictionary: withFilters)
        ps["$limit"] = "\(limit)"
        ps["$offset"] = "\(offset)"
        getResource(dataset, params: ps, completionHandler: completionHandler)
    }

    /// Queries a dataset using SoQL. See http://dev.socrata.com/docs/queries.html
    func queryDataset(dataset: String, withSoQL: String, limit: Int = SODADefaultLimit, offset: Int = 0, completionHandler: SODADatasetCompletionHandler) {
        let ps = NSMutableDictionary()
        ps["$where"] = "\(withSoQL)"
        ps["$limit"] = "\(limit)"
        ps["$offset"] = "\(offset)"
        getResource(dataset, params: ps, completionHandler: completionHandler)
    }
    
    /// Get a resource by its name and query parameters.
    private func getResource(resource: String, params: NSDictionary, completionHandler: SODADatasetCompletionHandler) {
        // Get the URL
        let query = SODAClient.paramsToQueryString (params)
        let path = resource.hasPrefix("/") ? resource : ("/resource/" + resource)
        
        let url = "https://\(self.domain)\(path).json?\(query)"
        let urlToSend = NSURL(string: url)
        
        // Build the request
        let request = NSMutableURLRequest(URL: urlToSend)
        request.addValue("application/json", forHTTPHeaderField:"Accept")
        request.addValue(self.token, forHTTPHeaderField:"X-App-Token")
        
        // Send it
        let session = NSURLSession.sharedSession()
        var task = session.dataTaskWithRequest(request, completionHandler: { data, response, reqError in
            
            // We sync the callback with the main thread to make UI programming easier
            let syncCompletion = { (r: SODADatasetResult) in
                NSOperationQueue.mainQueue().addOperationWithBlock({ () in completionHandler (r) })
            }
            
            // Give up if there was a net error
            if reqError != nil {
                syncCompletion (.Error (reqError))
                return
            }
            
            // Try to parse the JSON
//            println(NSString (data: data, encoding: NSUTF8StringEncoding))
            
            var jsonError: NSError?
            var jsonResult: AnyObject! = NSJSONSerialization.JSONObjectWithData(data, options: NSJSONReadingOptions.MutableContainers, error: &jsonError)
            if let error = jsonError {
                syncCompletion (.Error (error))
                return
            }
            
            // Interpret the JSON
            if let a = jsonResult as? [[String: AnyObject]] {
                syncCompletion (.Dataset (a))
            }
            else if let d = jsonResult as? [String: AnyObject] {
                if let e : AnyObject = d["error"] {
                    if let m : AnyObject = d["message"] {
                        syncCompletion (.Error (NSError(domain: "SODA", code: 0, userInfo: ["Error": m])))
                        return
                    }
                }
                syncCompletion (.Dataset ([d]))
            }
            else {
                syncCompletion (.Error (NSError()))
            }
        })
        task.resume()
    }
    
    /// Converts an NSDictionary into a query string.
    private class func paramsToQueryString (params: NSDictionary) -> String {
        var s = ""
        var head = ""
        for (key, value) in params {
            let sk = "\(key)".stringByAddingPercentEscapesUsingEncoding(NSUTF8StringEncoding)
            let sv = "\(value)".stringByAddingPercentEscapesUsingEncoding(NSUTF8StringEncoding)
            s += "\(head)\(sk)=\(sv)"
            head = "&"
        }
        return s
    }
}
