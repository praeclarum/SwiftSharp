namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type InheritanceTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
class Vehicle {
    var currentSpeed = 0.0
    var description: String {
        return "traveling at \(currentSpeed) miles per hour"
    }
    func makeNoise() {
        // do nothing - an arbitrary vehicle doesn't necessarily make a noise
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
let someVehicle = Vehicle()
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
println("Vehicle: \(someVehicle.description)")
// Vehicle: traveling at 0.0 miles per hour
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
class SomeSubclass: SomeSuperclass {
    // subclass definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
class Bicycle: Vehicle {
    var hasBasket = false
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
let bicycle = Bicycle()
bicycle.hasBasket = true
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
bicycle.currentSpeed = 15.0
println("Bicycle: \(bicycle.description)")
// Bicycle: traveling at 15.0 miles per hour
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
class Tandem: Bicycle {
    var currentNumberOfPassengers = 0
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
let tandem = Tandem()
tandem.hasBasket = true
tandem.currentNumberOfPassengers = 2
tandem.currentSpeed = 22.0
println("Tandem: \(tandem.description)")
// Tandem: traveling at 22.0 miles per hour
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
class Train: Vehicle {
    override func makeNoise() {
        println("Choo Choo")
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
let train = Train()
train.makeNoise()
// prints "Choo Choo"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
class Car: Vehicle {
    var gear = 1
    override var description: String {
        return super.description + " in gear \(gear)"
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
let car = Car()
car.currentSpeed = 25.0
car.gear = 3
println("Car: \(car.description)")
// Car: traveling at 25.0 miles per hour in gear 3
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
class AutomaticCar: Car {
    override var currentSpeed: Double {
        didSet {
            gear = Int(currentSpeed / 10.0) + 1
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
let automatic = AutomaticCar()
automatic.currentSpeed = 35.0
println("AutomaticCar: \(automatic.description)")
// AutomaticCar: traveling at 35.0 miles per hour in gear 4
        """
        this.Test (code)

