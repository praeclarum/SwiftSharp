namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ClosuresTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
let names = ["Chris", "Alex", "Ewa", "Barry", "Daniella"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
func backwards(s1: String, s2: String) -> Bool {
    return s1 > s2
}
var reversed = sorted(names, backwards)
// reversed is equal to ["Ewa", "Daniella", "Chris", "Barry", "Alex"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
reversed = sorted(names, { (s1: String, s2: String) -> Bool in
    return s1 > s2
})
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
reversed = sorted(names, { (s1: String, s2: String) -> Bool in return s1 > s2 } )
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
reversed = sorted(names, { s1, s2 in return s1 > s2 } )
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
reversed = sorted(names, { s1, s2 in s1 > s2 } )
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
reversed = sorted(names, { $0 > $1 } )
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
reversed = sorted(names, >)
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
func someFunctionThatTakesAClosure(closure: () -> ()) {
    // function body goes here
}
 
// here's how you call this function without using a trailing closure:
 
someFunctionThatTakesAClosure({
    // closure's body goes here
})
 
// here's how you call this function with a trailing closure instead:
 
someFunctionThatTakesAClosure() {
    // trailing closure's body goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
reversed = sorted(names) { $0 > $1 }
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
let digitNames = [
    0: "Zero", 1: "One", 2: "Two",   3: "Three", 4: "Four",
    5: "Five", 6: "Six", 7: "Seven", 8: "Eight", 9: "Nine"
]
let numbers = [16, 58, 510]
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
let strings = numbers.map {
    (var number) -> String in
    var output = ""
    while number > 0 {
        output = digitNames[number % 10]! + output
        number /= 10
    }
    return output
}
// strings is inferred to be of type [String]
// its value is ["OneSix", "FiveEight", "FiveOneZero"]
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
func makeIncrementor(forIncrement amount: Int) -> () -> Int {
    var runningTotal = 0
    func incrementor() -> Int {
        runningTotal += amount
        return runningTotal
    }
    return incrementor
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
func incrementor() -> Int {
    runningTotal += amount
    return runningTotal
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
let incrementByTen = makeIncrementor(forIncrement: 10)
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
incrementByTen()
// returns a value of 10
incrementByTen()
// returns a value of 20
incrementByTen()
// returns a value of 30
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
let incrementBySeven = makeIncrementor(forIncrement: 7)
incrementBySeven()
// returns a value of 7
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
incrementByTen()
// returns a value of 40
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
let alsoIncrementByTen = incrementByTen
alsoIncrementByTen()
// returns a value of 50
        """
        this.Test (code)

