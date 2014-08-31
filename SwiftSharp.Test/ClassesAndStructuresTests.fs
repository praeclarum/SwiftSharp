namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ClassesAndStructuresTests () =
    inherit BookTests ()

    [<Test>]
    member this.ClassesAndStructures01() =
        let code = """class SomeClass {
    // class definition goes here
}
struct SomeStructure {
    // structure definition goes here
} """
        this.Test ("ClassesAndStructures01", code)

    [<Test>]
    member this.ClassesAndStructures02() =
        let code = """struct Resolution {
    var width = 0
    var height = 0
}
class VideoMode {
    var resolution = Resolution()
    var interlaced = false
    var frameRate = 0.0
    var name: String?
} """
        this.Test ("ClassesAndStructures02", code)

    [<Test>]
    member this.ClassesAndStructures03() =
        let code = """let someResolution = Resolution()
let someVideoMode = VideoMode() """
        this.Test ("ClassesAndStructures03", code)

    [<Test>]
    member this.ClassesAndStructures04() =
        let code = """println("The width of someResolution is \(someResolution.width)")
// prints "The width of someResolution is 0" """
        this.Test ("ClassesAndStructures04", code)

    [<Test>]
    member this.ClassesAndStructures05() =
        let code = """println("The width of someVideoMode is \(someVideoMode.resolution.width)")
// prints "The width of someVideoMode is 0" """
        this.Test ("ClassesAndStructures05", code)

    [<Test>]
    member this.ClassesAndStructures06() =
        let code = """someVideoMode.resolution.width = 1280
println("The width of someVideoMode is now \(someVideoMode.resolution.width)")
// prints "The width of someVideoMode is now 1280" """
        this.Test ("ClassesAndStructures06", code)

    [<Test>]
    member this.ClassesAndStructures07() =
        let code = """let vga = Resolution(width: 640, height: 480) """
        this.Test ("ClassesAndStructures07", code)

    [<Test>]
    member this.ClassesAndStructures08() =
        let code = """let hd = Resolution(width: 1920, height: 1080)
var cinema = hd """
        this.Test ("ClassesAndStructures08", code)

    [<Test>]
    member this.ClassesAndStructures09() =
        let code = """cinema.width = 2048 """
        this.Test ("ClassesAndStructures09", code)

    [<Test>]
    member this.ClassesAndStructures10() =
        let code = """println("cinema is now \(cinema.width) pixels wide")
// prints "cinema is now 2048 pixels wide" """
        this.Test ("ClassesAndStructures10", code)

    [<Test>]
    member this.ClassesAndStructures11() =
        let code = """println("hd is still \(hd.width) pixels wide")
// prints "hd is still 1920 pixels wide" """
        this.Test ("ClassesAndStructures11", code)

    [<Test>]
    member this.ClassesAndStructures12() =
        let code = """enum CompassPoint {
    case North, South, East, West
}
var currentDirection = CompassPoint.West
let rememberedDirection = currentDirection
currentDirection = .East
if rememberedDirection == .West {
    println("The remembered direction is still .West")
}
// prints "The remembered direction is still .West" """
        this.Test ("ClassesAndStructures12", code)

    [<Test>]
    member this.ClassesAndStructures13() =
        let code = """let tenEighty = VideoMode()
tenEighty.resolution = hd
tenEighty.interlaced = true
tenEighty.name = "1080i"
tenEighty.frameRate = 25.0 """
        this.Test ("ClassesAndStructures13", code)

    [<Test>]
    member this.ClassesAndStructures14() =
        let code = """let alsoTenEighty = tenEighty
alsoTenEighty.frameRate = 30.0 """
        this.Test ("ClassesAndStructures14", code)

    [<Test>]
    member this.ClassesAndStructures15() =
        let code = """println("The frameRate property of tenEighty is now \(tenEighty.frameRate)")
// prints "The frameRate property of tenEighty is now 30.0" """
        this.Test ("ClassesAndStructures15", code)

    [<Test>]
    member this.ClassesAndStructures16() =
        let code = """if tenEighty === alsoTenEighty {
    println("tenEighty and alsoTenEighty refer to the same VideoMode instance.")
}
// prints "tenEighty and alsoTenEighty refer to the same VideoMode instance." """
        this.Test ("ClassesAndStructures16", code)

