namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ClosuresTests () =
    inherit BookTests ()

    [<Test>]
    member this.Closures01() =
        let code = """let names = ["Chris", "Alex", "Ewa", "Barry", "Daniella"] """
        this.Test ("Closures01", code)

    [<Test>]
    member this.Closures02() =
        let code = """func backwards(s1: String, s2: String) -> Bool {
    return s1 > s2
}
var reversed = sorted(names, backwards)
// reversed is equal to ["Ewa", "Daniella", "Chris", "Barry", "Alex"] """
        this.Test ("Closures02", code)

    [<Test>]
    member this.Closures03() =
        let code = """reversed = sorted(names, { (s1: String, s2: String) -> Bool in
    return s1 > s2
}) """
        this.Test ("Closures03", code)

    [<Test>]
    member this.Closures04() =
        let code = """reversed = sorted(names, { (s1: String, s2: String) -> Bool in return s1 > s2 } ) """
        this.Test ("Closures04", code)

    [<Test>]
    member this.Closures05() =
        let code = """reversed = sorted(names, { s1, s2 in return s1 > s2 } ) """
        this.Test ("Closures05", code)

    [<Test>]
    member this.Closures06() =
        let code = """reversed = sorted(names, { s1, s2 in s1 > s2 } ) """
        this.Test ("Closures06", code)

    [<Test>]
    member this.Closures07() =
        let code = """reversed = sorted(names, { $0 > $1 } ) """
        this.Test ("Closures07", code)

    [<Test>]
    member this.Closures08() =
        let code = """reversed = sorted(names, >) """
        this.Test ("Closures08", code)

    [<Test>]
    member this.Closures09() =
        let code = """func someFunctionThatTakesAClosure(closure: () -> ()) {
    // function body goes here
}
 
// here's how you call this function without using a trailing closure:
 
someFunctionThatTakesAClosure({
    // closure's body goes here
})
 
// here's how you call this function with a trailing closure instead:
 
someFunctionThatTakesAClosure() {
    // trailing closure's body goes here
} """
        this.Test ("Closures09", code)

    [<Test>]
    member this.Closures10() =
        let code = """reversed = sorted(names) { $0 > $1 } """
        this.Test ("Closures10", code)

    [<Test>]
    member this.Closures11() =
        let code = """let digitNames = [
    0: "Zero", 1: "One", 2: "Two",   3: "Three", 4: "Four",
    5: "Five", 6: "Six", 7: "Seven", 8: "Eight", 9: "Nine"
]
let numbers = [16, 58, 510] """
        this.Test ("Closures11", code)

    [<Test>]
    member this.Closures12() =
        let code = """let strings = numbers.map {
    (var number) -> String in
    var output = ""
    while number > 0 {
        output = digitNames[number % 10]! + output
        number /= 10
    }
    return output
}
// strings is inferred to be of type [String]
// its value is ["OneSix", "FiveEight", "FiveOneZero"] """
        this.Test ("Closures12", code)

    [<Test>]
    member this.Closures13() =
        let code = """func makeIncrementor(forIncrement amount: Int) -> () -> Int {
    var runningTotal = 0
    func incrementor() -> Int {
        runningTotal += amount
        return runningTotal
    }
    return incrementor
} """
        this.Test ("Closures13", code)

    [<Test>]
    member this.Closures14() =
        let code = """func incrementor() -> Int {
    runningTotal += amount
    return runningTotal
} """
        this.Test ("Closures14", code)

    [<Test>]
    member this.Closures15() =
        let code = """let incrementByTen = makeIncrementor(forIncrement: 10) """
        this.Test ("Closures15", code)

    [<Test>]
    member this.Closures16() =
        let code = """incrementByTen()
// returns a value of 10
incrementByTen()
// returns a value of 20
incrementByTen()
// returns a value of 30 """
        this.Test ("Closures16", code)

    [<Test>]
    member this.Closures17() =
        let code = """let incrementBySeven = makeIncrementor(forIncrement: 7)
incrementBySeven()
// returns a value of 7 """
        this.Test ("Closures17", code)

    [<Test>]
    member this.Closures18() =
        let code = """incrementByTen()
// returns a value of 40 """
        this.Test ("Closures18", code)

    [<Test>]
    member this.Closures19() =
        let code = """let alsoIncrementByTen = incrementByTen
alsoIncrementByTen()
// returns a value of 50 """
        this.Test ("Closures19", code)

