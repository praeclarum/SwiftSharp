namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type EnumerationsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
enum SomeEnumeration {
    // enumeration definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
enum CompassPoint {
    case North
    case South
    case East
    case West
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
enum Planet {
    case Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
var directionToHead = CompassPoint.West
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
directionToHead = .East
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
directionToHead = .South
switch directionToHead {
case .North:
    println("Lots of planets have a north")
case .South:
    println("Watch out for penguins")
case .East:
    println("Where the sun rises")
case .West:
    println("Where the skies are blue")
}
// prints "Watch out for penguins"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
let somePlanet = Planet.Earth
switch somePlanet {
case .Earth:
    println("Mostly harmless")
default:
    println("Not a safe place for humans")
}
// prints "Mostly harmless"
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
enum Barcode {
    case UPCA(Int, Int, Int, Int)
    case QRCode(String)
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
var productBarcode = Barcode.UPCA(8, 85909, 51226, 3)
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
productBarcode = .QRCode("ABCDEFGHIJKLMNOP")
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
switch productBarcode {
case .UPCA(let numberSystem, let manufacturer, let product, let check):
    println("UPC-A: \(numberSystem), \(manufacturer), \(product), \(check).")
case .QRCode(let productCode):
    println("QR code: \(productCode).")
}
// prints "QR code: ABCDEFGHIJKLMNOP."
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
switch productBarcode {
case let .UPCA(numberSystem, manufacturer, product, check):
    println("UPC-A: \(numberSystem), \(manufacturer), \(product), \(check).")
case let .QRCode(productCode):
    println("QR code: \(productCode).")
}
// prints "QR code: ABCDEFGHIJKLMNOP."
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
enum ASCIIControlCharacter: Character {
    case Tab = "\t"
    case LineFeed = "\n"
    case CarriageReturn = "\r"
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
enum Planet: Int {
    case Mercury = 1, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
let earthsOrder = Planet.Earth.toRaw()
// earthsOrder is 3
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
let possiblePlanet = Planet.fromRaw(7)
// possiblePlanet is of type Planet? and equals Planet.Uranus
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
let positionToFind = 9
if let somePlanet = Planet.fromRaw(positionToFind) {
    switch somePlanet {
    case .Earth:
        println("Mostly harmless")
    default:
        println("Not a safe place for humans")
    }
} else {
    println("There isn't a planet at position \(positionToFind)")
}
// prints "There isn't a planet at position 9"
        """
        this.Test (code)

