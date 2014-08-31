namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ExtensionsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Extensions01() =
        let code = """extension SomeType {
    // new functionality to add to SomeType goes here
} """
        this.Test ("Extensions01", code)

    [<Test>]
    member this.Extensions02() =
        let code = """extension SomeType: SomeProtocol, AnotherProtocol {
    // implementation of protocol requirements goes here
} """
        this.Test ("Extensions02", code)

    [<Test>]
    member this.Extensions03() =
        let code = """extension Double {
    var km: Double { return self * 1_000.0 }
    var m: Double { return self }
    var cm: Double { return self / 100.0 }
    var mm: Double { return self / 1_000.0 }
    var ft: Double { return self / 3.28084 }
}
let oneInch = 25.4.mm
println("One inch is \(oneInch) meters")
// prints "One inch is 0.0254 meters"
let threeFeet = 3.ft
println("Three feet is \(threeFeet) meters")
// prints "Three feet is 0.914399970739201 meters" """
        this.Test ("Extensions03", code)

    [<Test>]
    member this.Extensions04() =
        let code = """let aMarathon = 42.km + 195.m
println("A marathon is \(aMarathon) meters long")
// prints "A marathon is 42195.0 meters long" """
        this.Test ("Extensions04", code)

    [<Test>]
    member this.Extensions05() =
        let code = """struct Size {
    var width = 0.0, height = 0.0
}
struct Point {
    var x = 0.0, y = 0.0
}
struct Rect {
    var origin = Point()
    var size = Size()
} """
        this.Test ("Extensions05", code)

    [<Test>]
    member this.Extensions06() =
        let code = """let defaultRect = Rect()
let memberwiseRect = Rect(origin: Point(x: 2.0, y: 2.0),
    size: Size(width: 5.0, height: 5.0)) """
        this.Test ("Extensions06", code)

    [<Test>]
    member this.Extensions07() =
        let code = """extension Rect {
    init(center: Point, size: Size) {
        let originX = center.x - (size.width / 2)
        let originY = center.y - (size.height / 2)
        self.init(origin: Point(x: originX, y: originY), size: size)
    }
} """
        this.Test ("Extensions07", code)

    [<Test>]
    member this.Extensions08() =
        let code = """let centerRect = Rect(center: Point(x: 4.0, y: 4.0),
    size: Size(width: 3.0, height: 3.0))
// centerRect's origin is (2.5, 2.5) and its size is (3.0, 3.0) """
        this.Test ("Extensions08", code)

    [<Test>]
    member this.Extensions09() =
        let code = """extension Int {
    func repetitions(task: () -> ()) {
        for i in 0..<self {
            task()
        }
    }
} """
        this.Test ("Extensions09", code)

    [<Test>]
    member this.Extensions10() =
        let code = """3.repetitions({
    println("Hello!")
})
// Hello!
// Hello!
// Hello! """
        this.Test ("Extensions10", code)

    [<Test>]
    member this.Extensions11() =
        let code = """3.repetitions {
    println("Goodbye!")
}
// Goodbye!
// Goodbye!
// Goodbye! """
        this.Test ("Extensions11", code)

    [<Test>]
    member this.Extensions12() =
        let code = """extension Int {
    mutating func square() {
        self = self * self
    }
}
var someInt = 3
someInt.square()
// someInt is now 9 """
        this.Test ("Extensions12", code)

    [<Test>]
    member this.Extensions13() =
        let code = """extension Int {
    subscript(var digitIndex: Int) -> Int {
        var decimalBase = 1
            while digitIndex > 0 {
                decimalBase *= 10
                --digitIndex
            }
            return (self / decimalBase) % 10
    }
}
746381295[0]
// returns 5
746381295[1]
// returns 9
746381295[2]
// returns 2
746381295[8]
// returns 7 """
        this.Test ("Extensions13", code)

    [<Test>]
    member this.Extensions14() =
        let code = """746381295[9]
// returns 0, as if you had requested:
0746381295[9] """
        this.Test ("Extensions14", code)

    [<Test>]
    member this.Extensions15() =
        let code = """extension Int {
    enum Kind {
        case Negative, Zero, Positive
    }
    var kind: Kind {
        switch self {
        case 0:
            return .Zero
        case let x where x > 0:
            return .Positive
        default:
            return .Negative
            }
    }
} """
        this.Test ("Extensions15", code)

    [<Test>]
    member this.Extensions16() =
        let code = """func printIntegerKinds(numbers: [Int]) {
    for number in numbers {
        switch number.kind {
        case .Negative:
            print("- ")
        case .Zero:
            print("0 ")
        case .Positive:
            print("+ ")
        }
    }
    print("\n")
}
printIntegerKinds([3, 19, -27, 0, -6, 0, 7])
// prints "+ + - 0 - 0 +" """
        this.Test ("Extensions16", code)

