namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type CollectionTypesTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
var shoppingList: [String] = ["Eggs", "Milk"]
// shoppingList has been initialized with two initial items
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
var shoppingList = ["Eggs", "Milk"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
println("The shopping list contains \(shoppingList.count) items.")
// prints "The shopping list contains 2 items."
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
if shoppingList.isEmpty {
    println("The shopping list is empty.")
} else {
    println("The shopping list is not empty.")
}
// prints "The shopping list is not empty."
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
shoppingList.append("Flour")
// shoppingList now contains 3 items, and someone is making pancakes
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
shoppingList += ["Baking Powder"]
// shoppingList now contains 4 items
shoppingList += ["Chocolate Spread", "Cheese", "Butter"]
// shoppingList now contains 7 items
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
var firstItem = shoppingList[0]
// firstItem is equal to "Eggs"
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
shoppingList[0] = "Six eggs"
// the first item in the list is now equal to "Six eggs" rather than "Eggs"
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
shoppingList[4...6] = ["Bananas", "Apples"]
// shoppingList now contains 6 items
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
shoppingList.insert("Maple Syrup", atIndex: 0)
// shoppingList now contains 7 items
// "Maple Syrup" is now the first item in the list
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
let mapleSyrup = shoppingList.removeAtIndex(0)
// the item that was at index 0 has just been removed
// shoppingList now contains 6 items, and no Maple Syrup
// the mapleSyrup constant is now equal to the removed "Maple Syrup" string
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
firstItem = shoppingList[0]
// firstItem is now equal to "Six eggs"
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
let apples = shoppingList.removeLast()
// the last item in the array has just been removed
// shoppingList now contains 5 items, and no apples
// the apples constant is now equal to the removed "Apples" string
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
for item in shoppingList {
    println(item)
}
// Six eggs
// Milk
// Flour
// Baking Powder
// Bananas
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
for (index, value) in enumerate(shoppingList) {
    println("Item \(index + 1): \(value)")
}
// Item 1: Six eggs
// Item 2: Milk
// Item 3: Flour
// Item 4: Baking Powder
// Item 5: Bananas
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
var someInts = [Int]()
println("someInts is of type [Int] with \(someInts.count) items.")
// prints "someInts is of type [Int] with 0 items."
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
someInts.append(3)
// someInts now contains 1 value of type Int
someInts = []
// someInts is now an empty array, but is still of type [Int]
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
var threeDoubles = [Double](count: 3, repeatedValue: 0.0)
// threeDoubles is of type [Double], and equals [0.0, 0.0, 0.0]
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
var anotherThreeDoubles = [Double](count: 3, repeatedValue: 2.5)
// anotherThreeDoubles is inferred as [Double], and equals [2.5, 2.5, 2.5]
 
var sixDoubles = threeDoubles + anotherThreeDoubles
// sixDoubles is inferred as [Double], and equals [0.0, 0.0, 0.0, 2.5, 2.5, 2.5]
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
var airports: [String: String] = ["TYO": "Tokyo", "DUB": "Dublin"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
var airports = ["TYO": "Tokyo", "DUB": "Dublin"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
println("The airports dictionary contains \(airports.count) items.")
// prints "The airports dictionary contains 2 items."
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
if airports.isEmpty {
    println("The airports dictionary is empty.")
} else {
    println("The airports dictionary is not empty.")
}
// prints "The airports dictionary is not empty."
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
airports["LHR"] = "London"
// the airports dictionary now contains 3 items
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
airports["LHR"] = "London Heathrow"
// the value for "LHR" has been changed to "London Heathrow"
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
if let oldValue = airports.updateValue("Dublin International", forKey: "DUB") {
    println("The old value for DUB was \(oldValue).")
}
// prints "The old value for DUB was Dublin."
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
if let airportName = airports["DUB"] {
    println("The name of the airport is \(airportName).")
} else {
    println("That airport is not in the airports dictionary.")
}
// prints "The name of the airport is Dublin International."
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
airports["APL"] = "Apple International"
// "Apple International" is not the real airport for APL, so delete it
airports["APL"] = nil
// APL has now been removed from the dictionary
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
if let removedValue = airports.removeValueForKey("DUB") {
    println("The removed airport's name is \(removedValue).")
} else {
    println("The airports dictionary does not contain a value for DUB.")
}
// prints "The removed airport's name is Dublin International."
        """
        this.Test (code)

    [<Test>]
    member this.Sample30() =
        let code = """
for (airportCode, airportName) in airports {
    println("\(airportCode): \(airportName)")
}
// LHR: London Heathrow
// TYO: Tokyo
        """
        this.Test (code)

    [<Test>]
    member this.Sample31() =
        let code = """
for airportCode in airports.keys {
    println("Airport code: \(airportCode)")
}
// Airport code: LHR
// Airport code: TYO
 
for airportName in airports.values {
    println("Airport name: \(airportName)")
}
// Airport name: London Heathrow
// Airport name: Tokyo
        """
        this.Test (code)

    [<Test>]
    member this.Sample32() =
        let code = """
let airportCodes = [String](airports.keys)
// airportCodes is ["LHR", "TYO"]
 
let airportNames = [String](airports.values)
// airportNames is ["London Heathrow", "Tokyo"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample33() =
        let code = """
var namesOfIntegers = [Int: String]()
// namesOfIntegers is an empty [Int: String] dictionary
        """
        this.Test (code)

    [<Test>]
    member this.Sample34() =
        let code = """
namesOfIntegers[16] = "sixteen"
// namesOfIntegers now contains 1 key-value pair
namesOfIntegers = [:]
// namesOfIntegers is once again an empty dictionary of type [Int: String]
        """
        this.Test (code)

