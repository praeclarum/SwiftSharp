namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type BasicOperatorsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
let b = 10
var a = 5
a = b
// a is now equal to 10
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
let (x, y) = (1, 2)
// x is equal to 1, and y is equal to 2
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
if x = y {
    // this is not valid, because x = y does not return a value
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
1 + 2       // equals 3
5 - 3       // equals 2
2 * 3       // equals 6
10.0 / 2.5  // equals 4.0
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
"hello, " + "world"  // equals "hello, world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
let dog: Character = "🐶"
let cow: Character = "🐮"
let dogCow = dog + cow
// dogCow is equal to "🐶🐮"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
9 % 4    // equals 1
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
-9 % 4   // equals -1
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
8 % 2.5   // equals 0.5
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
var i = 0
++i      // i now equals 1
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
var a = 0
let b = ++a
// a and b are now both equal to 1
let c = a++
// a is now equal to 2, but c has been set to the pre-increment value of 1
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
let three = 3
let minusThree = -three       // minusThree equals -3
let plusThree = -minusThree   // plusThree equals 3, or "minus minus three"
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
let minusSix = -6
let alsoMinusSix = +minusSix  // alsoMinusSix equals -6
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
var a = 1
a += 2
// a is now equal to 3
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
1 == 1   // true, because 1 is equal to 1
2 != 1   // true, because 2 is not equal to 1
2 > 1    // true, because 2 is greater than 1
1 < 2    // true, because 1 is less than 2
1 >= 1   // true, because 1 is greater than or equal to 1
2 <= 1   // false, because 2 is not less than or equal to 1
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
let name = "world"
if name == "world" {
    println("hello, world")
} else {
    println("I'm sorry \(name), but I don't recognize you")
}
// prints "hello, world", because name is indeed equal to "world"
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
if question {
    answer1
} else {
    answer2
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
let contentHeight = 40
let hasHeader = true
let rowHeight = contentHeight + (hasHeader ? 50 : 20)
// rowHeight is equal to 90
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
let contentHeight = 40
let hasHeader = true
var rowHeight = contentHeight
if hasHeader {
    rowHeight = rowHeight + 50
} else {
    rowHeight = rowHeight + 20
}
// rowHeight is equal to 90
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
a != nil ? a! : b
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
let defaultColorName = "red"
var userDefinedColorName: String?   // defaults to nil
 
var colorNameToUse = userDefinedColorName ?? defaultColorName
// userDefinedColorName is nil, so colorNameToUse is set to the default of "red"
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
userDefinedColorName = "green"
colorNameToUse = userDefinedColorName ?? defaultColorName
// userDefinedColorName is not nil, so colorNameToUse is set to "green"
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
for index in 1...5 {
    println("\(index) times 5 is \(index * 5)")
}
// 1 times 5 is 5
// 2 times 5 is 10
// 3 times 5 is 15
// 4 times 5 is 20
// 5 times 5 is 25
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
let names = ["Anna", "Alex", "Brian", "Jack"]
let count = names.count
for i in 0..<count {
    println("Person \(i + 1) is called \(names[i])")
}
// Person 1 is called Anna
// Person 2 is called Alex
// Person 3 is called Brian
// Person 4 is called Jack
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
let allowedEntry = false
if !allowedEntry {
    println("ACCESS DENIED")
}
// prints "ACCESS DENIED"
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
let enteredDoorCode = true
let passedRetinaScan = false
if enteredDoorCode && passedRetinaScan {
    println("Welcome!")
} else {
    println("ACCESS DENIED")
}
// prints "ACCESS DENIED"
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
let hasDoorKey = false
let knowsOverridePassword = true
if hasDoorKey || knowsOverridePassword {
    println("Welcome!")
} else {
    println("ACCESS DENIED")
}
// prints "Welcome!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
if enteredDoorCode && passedRetinaScan || hasDoorKey || knowsOverridePassword {
    println("Welcome!")
} else {
    println("ACCESS DENIED")
}
// prints "Welcome!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
if (enteredDoorCode && passedRetinaScan) || hasDoorKey || knowsOverridePassword {
    println("Welcome!")
} else {
    println("ACCESS DENIED")
}
// prints "Welcome!"
        """
        this.Test (code)

