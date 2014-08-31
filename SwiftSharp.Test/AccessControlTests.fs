namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type AccessControlTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
public class SomePublicClass {}
internal class SomeInternalClass {}
private class SomePrivateClass {}
 
public var somePublicVariable = 0
internal let someInternalConstant = 0
private func somePrivateFunction() {}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
class SomeInternalClass {}              // implicitly internal
var someInternalConstant = 0            // implicitly internal
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
public class SomePublicClass {          // explicitly public class
    public var somePublicProperty = 0    // explicitly public class member
    var someInternalProperty = 0         // implicitly internal class member
    private func somePrivateMethod() {}  // explicitly private class member
}
 
class SomeInternalClass {               // implicitly internal class
    var someInternalProperty = 0         // implicitly internal class member
    private func somePrivateMethod() {}  // explicitly private class member
}
 
private class SomePrivateClass {        // explicitly private class
    var somePrivateProperty = 0          // implicitly private class member
    func somePrivateMethod() {}          // implicitly private class member
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
func someFunction() -> (SomeInternalClass, SomePrivateClass) {
    // function implementation goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
private func someFunction() -> (SomeInternalClass, SomePrivateClass) {
    // function implementation goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
public enum CompassPoint {
    case North
    case South
    case East
    case West
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
public class A {
    private func someMethod() {}
}
 
internal class B: A {
    override internal func someMethod() {}
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
public class A {
    private func someMethod() {}
}
 
internal class B: A {
    override internal func someMethod() {
        super.someMethod()
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
private var privateInstance = SomePrivateClass()
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
struct TrackedString {
    private(set) var numberOfEdits = 0
    var value: String = "" {
        didSet {
            numberOfEdits++
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
var stringToEdit = TrackedString()
stringToEdit.value = "This string will be tracked."
stringToEdit.value += " This edit will increment numberOfEdits."
stringToEdit.value += " So will this one."
println("The number of edits is \(stringToEdit.numberOfEdits)")
// prints "The number of edits is 3"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
public struct TrackedString {
    public private(set) var numberOfEdits = 0
    public var value: String = "" {
        didSet {
            numberOfEdits++
        }
    }
    public init() {}
}
        """
        this.Test (code)

