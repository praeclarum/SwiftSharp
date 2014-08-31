namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type GenericsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
func swapTwoInts(inout a: Int, inout b: Int) {
    let temporaryA = a
    a = b
    b = temporaryA
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
var someInt = 3
var anotherInt = 107
swapTwoInts(&someInt, &anotherInt)
println("someInt is now \(someInt), and anotherInt is now \(anotherInt)")
// prints "someInt is now 107, and anotherInt is now 3"
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
func swapTwoStrings(inout a: String, inout b: String) {
    let temporaryA = a
    a = b
    b = temporaryA
}
 
func swapTwoDoubles(inout a: Double, inout b: Double) {
    let temporaryA = a
    a = b
    b = temporaryA
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
func swapTwoValues<T>(inout a: T, inout b: T) {
    let temporaryA = a
    a = b
    b = temporaryA
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
func swapTwoInts(inout a: Int, inout b: Int)
func swapTwoValues<T>(inout a: T, inout b: T)
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
var someInt = 3
var anotherInt = 107
swapTwoValues(&someInt, &anotherInt)
// someInt is now 107, and anotherInt is now 3
 
var someString = "hello"
var anotherString = "world"
swapTwoValues(&someString, &anotherString)
// someString is now "world", and anotherString is now "hello"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
struct IntStack {
    var items = [Int]()
    mutating func push(item: Int) {
        items.append(item)
    }
    mutating func pop() -> Int {
        return items.removeLast()
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
struct Stack<T> {
    var items = [T]()
    mutating func push(item: T) {
        items.append(item)
    }
    mutating func pop() -> T {
        return items.removeLast()
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
var stackOfStrings = Stack<String>()
stackOfStrings.push("uno")
stackOfStrings.push("dos")
stackOfStrings.push("tres")
stackOfStrings.push("cuatro")
// the stack now contains 4 strings
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
let fromTheTop = stackOfStrings.pop()
// fromTheTop is equal to "cuatro", and the stack now contains 3 strings
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
extension Stack {
    var topItem: T? {
        return items.isEmpty ? nil : items[items.count - 1]
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
if let topItem = stackOfStrings.topItem {
    println("The top item on the stack is \(topItem).")
}
// prints "The top item on the stack is tres."
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
func someFunction<T: SomeClass, U: SomeProtocol>(someT: T, someU: U) {
    // function body goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
func findStringIndex(array: [String], valueToFind: String) -> Int? {
    for (index, value) in enumerate(array) {
        if value == valueToFind {
            return index
        }
    }
    return nil
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
let strings = ["cat", "dog", "llama", "parakeet", "terrapin"]
if let foundIndex = findStringIndex(strings, "llama") {
    println("The index of llama is \(foundIndex)")
}
// prints "The index of llama is 2"
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
func findIndex<T>(array: [T], valueToFind: T) -> Int? {
    for (index, value) in enumerate(array) {
        if value == valueToFind {
            return index
        }
    }
    return nil
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
func findIndex<T: Equatable>(array: [T], valueToFind: T) -> Int? {
    for (index, value) in enumerate(array) {
        if value == valueToFind {
            return index
        }
    }
    return nil
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
let doubleIndex = findIndex([3.14159, 0.1, 0.25], 9.3)
// doubleIndex is an optional Int with no value, because 9.3 is not in the array
let stringIndex = findIndex(["Mike", "Malcolm", "Andrea"], "Andrea")
// stringIndex is an optional Int containing a value of 2
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
protocol Container {
    typealias ItemType
    mutating func append(item: ItemType)
    var count: Int { get }
    subscript(i: Int) -> ItemType { get }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
struct IntStack: Container {
    // original IntStack implementation
    var items = [Int]()
    mutating func push(item: Int) {
        items.append(item)
    }
    mutating func pop() -> Int {
        return items.removeLast()
    }
    // conformance to the Container protocol
    typealias ItemType = Int
    mutating func append(item: Int) {
        self.push(item)
    }
    var count: Int {
        return items.count
    }
    subscript(i: Int) -> Int {
        return items[i]
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
struct Stack<T>: Container {
    // original Stack<T> implementation
    var items = [T]()
    mutating func push(item: T) {
        items.append(item)
    }
    mutating func pop() -> T {
        return items.removeLast()
    }
    // conformance to the Container protocol
    mutating func append(item: T) {
        self.push(item)
    }
    var count: Int {
        return items.count
    }
    subscript(i: Int) -> T {
        return items[i]
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
extension Array: Container {}
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
func allItemsMatch<
    C1: Container, C2: Container
    where C1.ItemType == C2.ItemType, C1.ItemType: Equatable>
    (someContainer: C1, anotherContainer: C2) -> Bool {
        
        // check that both containers contain the same number of items
        if someContainer.count != anotherContainer.count {
            return false
        }
        
        // check each pair of items to see if they are equivalent
        for i in 0..<someContainer.count {
            if someContainer[i] != anotherContainer[i] {
                return false
            }
        }
        
        // all items match, so return true
        return true
        
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
var stackOfStrings = Stack<String>()
stackOfStrings.push("uno")
stackOfStrings.push("dos")
stackOfStrings.push("tres")
 
var arrayOfStrings = ["uno", "dos", "tres"]
 
if allItemsMatch(stackOfStrings, arrayOfStrings) {
    println("All items match.")
} else {
    println("Not all items match.")
}
// prints "All items match."
        """
        this.Test (code)

