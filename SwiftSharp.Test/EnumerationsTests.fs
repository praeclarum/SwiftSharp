namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type EnumerationsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Enumerations01() =
        let code = """enum SomeEnumeration {
    // enumeration definition goes here
} """
        this.Test ("Enumerations01", code)

    [<Test>]
    member this.Enumerations02() =
        let code = """enum CompassPoint {
    case North
    case South
    case East
    case West
} """
        this.Test ("Enumerations02", code)

    [<Test>]
    member this.Enumerations03() =
        let code = """enum Planet {
    case Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune
} """
        this.Test ("Enumerations03", code)

    [<Test>]
    member this.Enumerations04() =
        let code = """var directionToHead = CompassPoint.West """
        this.Test ("Enumerations04", code)

    [<Test>]
    member this.Enumerations05() =
        let code = """directionToHead = .East """
        this.Test ("Enumerations05", code)

    [<Test>]
    member this.Enumerations06() =
        let code = """directionToHead = .South
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
// prints "Watch out for penguins" """
        this.Test ("Enumerations06", code)

    [<Test>]
    member this.Enumerations07() =
        let code = """let somePlanet = Planet.Earth
switch somePlanet {
case .Earth:
    println("Mostly harmless")
default:
    println("Not a safe place for humans")
}
// prints "Mostly harmless" """
        this.Test ("Enumerations07", code)

    [<Test>]
    member this.Enumerations08() =
        let code = """enum Barcode {
    case UPCA(Int, Int, Int, Int)
    case QRCode(String)
} """
        this.Test ("Enumerations08", code)

    [<Test>]
    member this.Enumerations09() =
        let code = """var productBarcode = Barcode.UPCA(8, 85909, 51226, 3) """
        this.Test ("Enumerations09", code)

    [<Test>]
    member this.Enumerations10() =
        let code = """productBarcode = .QRCode("ABCDEFGHIJKLMNOP") """
        this.Test ("Enumerations10", code)

    [<Test>]
    member this.Enumerations11() =
        let code = """switch productBarcode {
case .UPCA(let numberSystem, let manufacturer, let product, let check):
    println("UPC-A: \(numberSystem), \(manufacturer), \(product), \(check).")
case .QRCode(let productCode):
    println("QR code: \(productCode).")
}
// prints "QR code: ABCDEFGHIJKLMNOP." """
        this.Test ("Enumerations11", code)

    [<Test>]
    member this.Enumerations12() =
        let code = """switch productBarcode {
case let .UPCA(numberSystem, manufacturer, product, check):
    println("UPC-A: \(numberSystem), \(manufacturer), \(product), \(check).")
case let .QRCode(productCode):
    println("QR code: \(productCode).")
}
// prints "QR code: ABCDEFGHIJKLMNOP." """
        this.Test ("Enumerations12", code)

    [<Test>]
    member this.Enumerations13() =
        let code = """enum ASCIIControlCharacter: Character {
    case Tab = "\t"
    case LineFeed = "\n"
    case CarriageReturn = "\r"
} """
        this.Test ("Enumerations13", code)

    [<Test>]
    member this.Enumerations14() =
        let code = """enum Planet: Int {
    case Mercury = 1, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune
} """
        this.Test ("Enumerations14", code)

    [<Test>]
    member this.Enumerations15() =
        let code = """let earthsOrder = Planet.Earth.toRaw()
// earthsOrder is 3 """
        this.Test ("Enumerations15", code)

    [<Test>]
    member this.Enumerations16() =
        let code = """let possiblePlanet = Planet.fromRaw(7)
// possiblePlanet is of type Planet? and equals Planet.Uranus """
        this.Test ("Enumerations16", code)

    [<Test>]
    member this.Enumerations17() =
        let code = """let positionToFind = 9
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
// prints "There isn't a planet at position 9" """
        this.Test ("Enumerations17", code)

