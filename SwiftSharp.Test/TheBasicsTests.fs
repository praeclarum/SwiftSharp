namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type TheBasicsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
let maximumNumberOfLoginAttempts = 10
var currentLoginAttempt = 0
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
var x = 0.0, y = 0.0, z = 0.0
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
var welcomeMessage: String
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
welcomeMessage = "Hello"
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
var red, green, blue: Double
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
let π = 3.14159
let 你好 = "你好世界"
let 🐶🐮 = "dogcow"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
var friendlyWelcome = "Hello!"
friendlyWelcome = "Bonjour!"
// friendlyWelcome is now "Bonjour!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
let languageName = "Swift"
languageName = "Swift++"
// this is a compile-time error - languageName cannot be changed
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
println(friendlyWelcome)
// prints "Bonjour!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
println("This is a string")
// prints "This is a string"
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
println("The current value of friendlyWelcome is \(friendlyWelcome)")
// prints "The current value of friendlyWelcome is Bonjour!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
// this is a comment
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
/* this is also a comment,
but written over multiple lines */
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
/* this is the start of the first multiline comment
/* this is the second, nested multiline comment */
this is the end of the first multiline comment */
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
let cat = "🐱"; println(cat)
// prints "🐱"
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
let minValue = UInt8.min  // minValue is equal to 0, and is of type UInt8
let maxValue = UInt8.max  // maxValue is equal to 255, and is of type UInt8
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
let meaningOfLife = 42
// meaningOfLife is inferred to be of type Int
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
let pi = 3.14159
// pi is inferred to be of type Double
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
let anotherPi = 3 + 0.14159
// anotherPi is also inferred to be of type Double
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
let decimalInteger = 17
let binaryInteger = 0b10001       // 17 in binary notation
let octalInteger = 0o21           // 17 in octal notation
let hexadecimalInteger = 0x11     // 17 in hexadecimal notation
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
let decimalDouble = 12.1875
let exponentDouble = 1.21875e1
let hexadecimalDouble = 0xC.3p0
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
let paddedDouble = 000123.456
let oneMillion = 1_000_000
let justOverOneMillion = 1_000_000.000_000_1
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
let cannotBeNegative: UInt8 = -1
// UInt8 cannot store negative numbers, and so this will report an error
let tooBig: Int8 = Int8.max + 1
// Int8 cannot store a number larger than its maximum value,
// and so this will also report an error
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
let twoThousand: UInt16 = 2_000
let one: UInt8 = 1
let twoThousandAndOne = twoThousand + UInt16(one)
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
let three = 3
let pointOneFourOneFiveNine = 0.14159
let pi = Double(three) + pointOneFourOneFiveNine
// pi equals 3.14159, and is inferred to be of type Double
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
let integerPi = Int(pi)
// integerPi equals 3, and is inferred to be of type Int
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
typealias AudioSample = UInt16
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
var maxAmplitudeFound = AudioSample.min
// maxAmplitudeFound is now 0
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
let orangesAreOrange = true
let turnipsAreDelicious = false
        """
        this.Test (code)

    [<Test>]
    member this.Sample30() =
        let code = """
if turnipsAreDelicious {
    println("Mmm, tasty turnips!")
} else {
    println("Eww, turnips are horrible.")
}
// prints "Eww, turnips are horrible."
        """
        this.Test (code)

    [<Test>]
    member this.Sample31() =
        let code = """
let i = 1
if i {
    // this example will not compile, and will report an error
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample32() =
        let code = """
let i = 1
if i == 1 {
    // this example will compile successfully
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample33() =
        let code = """
let http404Error = (404, "Not Found")
// http404Error is of type (Int, String), and equals (404, "Not Found")
        """
        this.Test (code)

    [<Test>]
    member this.Sample34() =
        let code = """
let (statusCode, statusMessage) = http404Error
println("The status code is \(statusCode)")
// prints "The status code is 404"
println("The status message is \(statusMessage)")
// prints "The status message is Not Found"
        """
        this.Test (code)

    [<Test>]
    member this.Sample35() =
        let code = """
let (justTheStatusCode, _) = http404Error
println("The status code is \(justTheStatusCode)")
// prints "The status code is 404"
        """
        this.Test (code)

    [<Test>]
    member this.Sample36() =
        let code = """
println("The status code is \(http404Error.0)")
// prints "The status code is 404"
println("The status message is \(http404Error.1)")
// prints "The status message is Not Found"
        """
        this.Test (code)

    [<Test>]
    member this.Sample37() =
        let code = """
let http200Status = (statusCode: 200, description: "OK")
        """
        this.Test (code)

    [<Test>]
    member this.Sample38() =
        let code = """
println("The status code is \(http200Status.statusCode)")
// prints "The status code is 200"
println("The status message is \(http200Status.description)")
// prints "The status message is OK"
        """
        this.Test (code)

    [<Test>]
    member this.Sample39() =
        let code = """
let possibleNumber = "123"
let convertedNumber = possibleNumber.toInt()
// convertedNumber is inferred to be of type "Int?", or "optional Int"
        """
        this.Test (code)

    [<Test>]
    member this.Sample40() =
        let code = """
var serverResponseCode: Int? = 404
// serverResponseCode contains an actual Int value of 404
serverResponseCode = nil
// serverResponseCode now contains no value
        """
        this.Test (code)

    [<Test>]
    member this.Sample41() =
        let code = """
var surveyAnswer: String?
// surveyAnswer is automatically set to nil
        """
        this.Test (code)

    [<Test>]
    member this.Sample42() =
        let code = """
if convertedNumber != nil {
    println("convertedNumber contains some integer value.")
}
// prints "convertedNumber contains some integer value."
        """
        this.Test (code)

    [<Test>]
    member this.Sample43() =
        let code = """
if convertedNumber != nil {
    println("convertedNumber has an integer value of \(convertedNumber!).")
}
// prints "convertedNumber has an integer value of 123."
        """
        this.Test (code)

    [<Test>]
    member this.Sample44() =
        let code = """
if let actualNumber = possibleNumber.toInt() {
    println("\(possibleNumber) has an integer value of \(actualNumber)")
} else {
    println("\(possibleNumber) could not be converted to an integer")
}
// prints "123 has an integer value of 123"
        """
        this.Test (code)

    [<Test>]
    member this.Sample45() =
        let code = """
let possibleString: String? = "An optional string."
let forcedString: String = possibleString! // requires an exclamation mark
 
let assumedString: String! = "An implicitly unwrapped optional string."
let implicitString: String = assumedString // no need for an exclamation mark
        """
        this.Test (code)

    [<Test>]
    member this.Sample46() =
        let code = """
if assumedString != nil {
    println(assumedString)
}
// prints "An implicitly unwrapped optional string."
        """
        this.Test (code)

    [<Test>]
    member this.Sample47() =
        let code = """
if let definiteString = assumedString {
    println(definiteString)
}
// prints "An implicitly unwrapped optional string."
        """
        this.Test (code)

    [<Test>]
    member this.Sample48() =
        let code = """
let age = -3
assert(age >= 0, "A person's age cannot be less than zero")
// this causes the assertion to trigger, because age is not >= 0
        """
        this.Test (code)

    [<Test>]
    member this.Sample49() =
        let code = """
assert(age >= 0)
        """
        this.Test (code)

