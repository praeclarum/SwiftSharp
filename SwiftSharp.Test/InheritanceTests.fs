namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type InheritanceTests () =
    inherit BookTests ()

    [<Test>]
    member this.Inheritance01() =
        let code = """class Vehicle {
    var currentSpeed = 0.0
    var description: String {
        return "traveling at \(currentSpeed) miles per hour"
    }
    func makeNoise() {
        // do nothing - an arbitrary vehicle doesn't necessarily make a noise
    }
} """
        this.Test ("Inheritance01", code)

    [<Test>]
    member this.Inheritance02() =
        let code = """let someVehicle = Vehicle() """
        this.Test ("Inheritance02", code)

    [<Test>]
    member this.Inheritance03() =
        let code = """println("Vehicle: \(someVehicle.description)")
// Vehicle: traveling at 0.0 miles per hour """
        this.Test ("Inheritance03", code)

    [<Test>]
    member this.Inheritance04() =
        let code = """class SomeSubclass: SomeSuperclass {
    // subclass definition goes here
} """
        this.Test ("Inheritance04", code)

    [<Test>]
    member this.Inheritance05() =
        let code = """class Bicycle: Vehicle {
    var hasBasket = false
} """
        this.Test ("Inheritance05", code)

    [<Test>]
    member this.Inheritance06() =
        let code = """let bicycle = Bicycle()
bicycle.hasBasket = true """
        this.Test ("Inheritance06", code)

    [<Test>]
    member this.Inheritance07() =
        let code = """bicycle.currentSpeed = 15.0
println("Bicycle: \(bicycle.description)")
// Bicycle: traveling at 15.0 miles per hour """
        this.Test ("Inheritance07", code)

    [<Test>]
    member this.Inheritance08() =
        let code = """class Tandem: Bicycle {
    var currentNumberOfPassengers = 0
} """
        this.Test ("Inheritance08", code)

    [<Test>]
    member this.Inheritance09() =
        let code = """let tandem = Tandem()
tandem.hasBasket = true
tandem.currentNumberOfPassengers = 2
tandem.currentSpeed = 22.0
println("Tandem: \(tandem.description)")
// Tandem: traveling at 22.0 miles per hour """
        this.Test ("Inheritance09", code)

    [<Test>]
    member this.Inheritance10() =
        let code = """class Train: Vehicle {
    override func makeNoise() {
        println("Choo Choo")
    }
} """
        this.Test ("Inheritance10", code)

    [<Test>]
    member this.Inheritance11() =
        let code = """let train = Train()
train.makeNoise()
// prints "Choo Choo" """
        this.Test ("Inheritance11", code)

    [<Test>]
    member this.Inheritance12() =
        let code = """class Car: Vehicle {
    var gear = 1
    override var description: String {
        return super.description + " in gear \(gear)"
    }
} """
        this.Test ("Inheritance12", code)

    [<Test>]
    member this.Inheritance13() =
        let code = """let car = Car()
car.currentSpeed = 25.0
car.gear = 3
println("Car: \(car.description)")
// Car: traveling at 25.0 miles per hour in gear 3 """
        this.Test ("Inheritance13", code)

    [<Test>]
    member this.Inheritance14() =
        let code = """class AutomaticCar: Car {
    override var currentSpeed: Double {
        didSet {
            gear = Int(currentSpeed / 10.0) + 1
        }
    }
} """
        this.Test ("Inheritance14", code)

    [<Test>]
    member this.Inheritance15() =
        let code = """let automatic = AutomaticCar()
automatic.currentSpeed = 35.0
println("AutomaticCar: \(automatic.description)")
// AutomaticCar: traveling at 35.0 miles per hour in gear 4 """
        this.Test ("Inheritance15", code)

