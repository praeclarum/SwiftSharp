namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ClassesAndStructuresTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
class SomeClass {
    // class definition goes here
}
struct SomeStructure {
    // structure definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
struct Resolution {
    var width = 0
    var height = 0
}
class VideoMode {
    var resolution = Resolution()
    var interlaced = false
    var frameRate = 0.0
    var name: String?
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
let someResolution = Resolution()
let someVideoMode = VideoMode()
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
println("The width of someResolution is \(someResolution.width)")
// prints "The width of someResolution is 0"
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
println("The width of someVideoMode is \(someVideoMode.resolution.width)")
// prints "The width of someVideoMode is 0"
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
someVideoMode.resolution.width = 1280
println("The width of someVideoMode is now \(someVideoMode.resolution.width)")
// prints "The width of someVideoMode is now 1280"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
let vga = Resolution(width: 640, height: 480)
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
let hd = Resolution(width: 1920, height: 1080)
var cinema = hd
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
cinema.width = 2048
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
println("cinema is now \(cinema.width) pixels wide")
// prints "cinema is now 2048 pixels wide"
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
println("hd is still \(hd.width) pixels wide")
// prints "hd is still 1920 pixels wide"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
enum CompassPoint {
    case North, South, East, West
}
var currentDirection = CompassPoint.West
let rememberedDirection = currentDirection
currentDirection = .East
if rememberedDirection == .West {
    println("The remembered direction is still .West")
}
// prints "The remembered direction is still .West"
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
let tenEighty = VideoMode()
tenEighty.resolution = hd
tenEighty.interlaced = true
tenEighty.name = "1080i"
tenEighty.frameRate = 25.0
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
let alsoTenEighty = tenEighty
alsoTenEighty.frameRate = 30.0
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
println("The frameRate property of tenEighty is now \(tenEighty.frameRate)")
// prints "The frameRate property of tenEighty is now 30.0"
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
if tenEighty === alsoTenEighty {
    println("tenEighty and alsoTenEighty refer to the same VideoMode instance.")
}
// prints "tenEighty and alsoTenEighty refer to the same VideoMode instance."
        """
        this.Test (code)

