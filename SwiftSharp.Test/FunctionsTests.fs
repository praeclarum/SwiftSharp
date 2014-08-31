namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type FunctionsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
func sayHello(personName: String) -> String {
    let greeting = "Hello, " + personName + "!"
    return greeting
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
println(sayHello("Anna"))
// prints "Hello, Anna!"
println(sayHello("Brian"))
// prints "Hello, Brian!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
func sayHelloAgain(personName: String) -> String {
    return "Hello again, " + personName + "!"
}
println(sayHelloAgain("Anna"))
// prints "Hello again, Anna!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
func halfOpenRangeLength(start: Int, end: Int) -> Int {
    return end - start
}
println(halfOpenRangeLength(1, 10))
// prints "9"
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
func sayHelloWorld() -> String {
    return "hello, world"
}
println(sayHelloWorld())
// prints "hello, world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
func sayGoodbye(personName: String) {
    println("Goodbye, \(personName)!")
}
sayGoodbye("Dave")
// prints "Goodbye, Dave!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
func printAndCount(stringToPrint: String) -> Int {
    println(stringToPrint)
    return countElements(stringToPrint)
}
func printWithoutCounting(stringToPrint: String) {
    printAndCount(stringToPrint)
}
printAndCount("hello, world")
// prints "hello, world" and returns a value of 12
printWithoutCounting("hello, world")
// prints "hello, world" but does not return a value
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
func minMax(array: [Int]) -> (min: Int, max: Int) {
    var currentMin = array[0]
    var currentMax = array[0]
    for value in array[1..<array.count] {
        if value < currentMin {
            currentMin = value
        } else if value > currentMax {
            currentMax = value
        }
    }
    return (currentMin, currentMax)
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
let bounds = minMax([8, -6, 2, 109, 3, 71])
println("min is \(bounds.min) and max is \(bounds.max)")
// prints "min is -6 and max is 109"
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
func minMax(array: [Int]) -> (min: Int, max: Int)? {
    if array.isEmpty { return nil }
    var currentMin = array[0]
    var currentMax = array[0]
    for value in array[1..<array.count] {
        if value < currentMin {
            currentMin = value
        } else if value > currentMax {
            currentMax = value
        }
    }
    return (currentMin, currentMax)
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
if let bounds = minMax([8, -6, 2, 109, 3, 71]) {
    println("min is \(bounds.min) and max is \(bounds.max)")
}
// prints "min is -6 and max is 109"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
func someFunction(parameterName: Int) {
    // function body goes here, and can use parameterName
    // to refer to the argument value for that parameter
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
func someFunction(externalParameterName localParameterName: Int) {
    // function body goes here, and can use localParameterName
    // to refer to the argument value for that parameter
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
func join(s1: String, s2: String, joiner: String) -> String {
    return s1 + joiner + s2
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
join("hello", "world", ", ")
// returns "hello, world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
func join(string s1: String, toString s2: String, withJoiner joiner: String)
    -> String {
        return s1 + joiner + s2
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
join(string: "hello", toString: "world", withJoiner: ", ")
// returns "hello, world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
func containsCharacter(#string: String, #characterToFind: Character) -> Bool {
    for character in string {
        if character == characterToFind {
            return true
        }
    }
    return false
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
let containsAVee = containsCharacter(string: "aardvark", characterToFind: "v")
// containsAVee equals true, because "aardvark" contains a "v"
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
func join(string s1: String, toString s2: String,
    withJoiner joiner: String = " ") -> String {
        return s1 + joiner + s2
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
join(string: "hello", toString: "world", withJoiner: "-")
// returns "hello-world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
join(string: "hello", toString: "world")
// returns "hello world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
func join(s1: String, s2: String, joiner: String = " ") -> String {
    return s1 + joiner + s2
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
join("hello", "world", joiner: "-")
// returns "hello-world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
func arithmeticMean(numbers: Double...) -> Double {
    var total: Double = 0
    for number in numbers {
        total += number
    }
    return total / Double(numbers.count)
}
arithmeticMean(1, 2, 3, 4, 5)
// returns 3.0, which is the arithmetic mean of these five numbers
arithmeticMean(3, 8.25, 18.75)
// returns 10.0, which is the arithmetic mean of these three numbers
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
func alignRight(var string: String, count: Int, pad: Character) -> String {
    let amountToPad = count - countElements(string)
    if amountToPad < 1 {
        return string
    }
    let padString = String(pad)
    for _ in 1...amountToPad {
        string = padString + string
    }
    return string
}
let originalString = "hello"
let paddedString = alignRight(originalString, 10, "-")
// paddedString is equal to "-----hello"
// originalString is still equal to "hello"
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
func swapTwoInts(inout a: Int, inout b: Int) {
    let temporaryA = a
    a = b
    b = temporaryA
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
var someInt = 3
var anotherInt = 107
swapTwoInts(&someInt, &anotherInt)
println("someInt is now \(someInt), and anotherInt is now \(anotherInt)")
// prints "someInt is now 107, and anotherInt is now 3"
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
func addTwoInts(a: Int, b: Int) -> Int {
    return a + b
}
func multiplyTwoInts(a: Int, b: Int) -> Int {
    return a * b
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample30() =
        let code = """
func printHelloWorld() {
    println("hello, world")
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample31() =
        let code = """
var mathFunction: (Int, Int) -> Int = addTwoInts
        """
        this.Test (code)

    [<Test>]
    member this.Sample32() =
        let code = """
println("Result: \(mathFunction(2, 3))")
// prints "Result: 5"
        """
        this.Test (code)

    [<Test>]
    member this.Sample33() =
        let code = """
mathFunction = multiplyTwoInts
println("Result: \(mathFunction(2, 3))")
// prints "Result: 6"
        """
        this.Test (code)

    [<Test>]
    member this.Sample34() =
        let code = """
let anotherMathFunction = addTwoInts
// anotherMathFunction is inferred to be of type (Int, Int) -> Int
        """
        this.Test (code)

    [<Test>]
    member this.Sample35() =
        let code = """
func printMathResult(mathFunction: (Int, Int) -> Int, a: Int, b: Int) {
    println("Result: \(mathFunction(a, b))")
}
printMathResult(addTwoInts, 3, 5)
// prints "Result: 8"
        """
        this.Test (code)

    [<Test>]
    member this.Sample36() =
        let code = """
func stepForward(input: Int) -> Int {
    return input + 1
}
func stepBackward(input: Int) -> Int {
    return input - 1
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample37() =
        let code = """
func chooseStepFunction(backwards: Bool) -> (Int) -> Int {
    return backwards ? stepBackward : stepForward
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample38() =
        let code = """
var currentValue = 3
let moveNearerToZero = chooseStepFunction(currentValue > 0)
// moveNearerToZero now refers to the stepBackward() function
        """
        this.Test (code)

    [<Test>]
    member this.Sample39() =
        let code = """
println("Counting to zero:")
// Counting to zero:
while currentValue != 0 {
    println("\(currentValue)... ")
    currentValue = moveNearerToZero(currentValue)
}
println("zero!")
// 3...
// 2...
// 1...
// zero!
        """
        this.Test (code)

    [<Test>]
    member this.Sample40() =
        let code = """
func chooseStepFunction(backwards: Bool) -> (Int) -> Int {
    func stepForward(input: Int) -> Int { return input + 1 }
    func stepBackward(input: Int) -> Int { return input - 1 }
    return backwards ? stepBackward : stepForward
}
var currentValue = -4
let moveNearerToZero = chooseStepFunction(currentValue > 0)
// moveNearerToZero now refers to the nested stepForward() function
while currentValue != 0 {
    println("\(currentValue)... ")
    currentValue = moveNearerToZero(currentValue)
}
println("zero!")
// -4...
// -3...
// -2...
// -1...
// zero!
        """
        this.Test (code)

