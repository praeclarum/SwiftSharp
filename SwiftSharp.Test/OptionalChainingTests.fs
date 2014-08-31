namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type OptionalChainingTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
class Person {
    var residence: Residence?
}
 
class Residence {
    var numberOfRooms = 1
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
let john = Person()
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
let roomCount = john.residence!.numberOfRooms
// this triggers a runtime error
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "Unable to retrieve the number of rooms."
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
john.residence = Residence()
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "John's residence has 1 room(s)."
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
class Person {
    var residence: Residence?
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
class Residence {
    var rooms = [Room]()
    var numberOfRooms: Int {
        return rooms.count
    }
    subscript(i: Int) -> Room {
        get {
            return rooms[i]
        }
        set {
            rooms[i] = newValue
        }
    }
    func printNumberOfRooms() {
        println("The number of rooms is \(numberOfRooms)")
    }
    var address: Address?
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
class Room {
    let name: String
    init(name: String) { self.name = name }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
class Address {
    var buildingName: String?
    var buildingNumber: String?
    var street: String?
    func buildingIdentifier() -> String? {
        if buildingName != nil {
            return buildingName
        } else if buildingNumber != nil {
            return buildingNumber
        } else {
            return nil
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
let john = Person()
if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "Unable to retrieve the number of rooms."
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
let someAddress = Address()
someAddress.buildingNumber = "29"
someAddress.street = "Acacia Road"
john.residence?.address = someAddress
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
func printNumberOfRooms() {
    println("The number of rooms is \(numberOfRooms)")
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
if john.residence?.printNumberOfRooms() != nil {
    println("It was possible to print the number of rooms.")
} else {
    println("It was not possible to print the number of rooms.")
}
// prints "It was not possible to print the number of rooms."
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
if (john.residence?.address = someAddress) != nil {
    println("It was possible to set the address.")
} else {
    println("It was not possible to set the address.")
}
// prints "It was not possible to set the address."
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
if let firstRoomName = john.residence?[0].name {
    println("The first room name is \(firstRoomName).")
} else {
    println("Unable to retrieve the first room name.")
}
// prints "Unable to retrieve the first room name."
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
john.residence?[0] = Room(name: "Bathroom")
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
let johnsHouse = Residence()
johnsHouse.rooms.append(Room(name: "Living Room"))
johnsHouse.rooms.append(Room(name: "Kitchen"))
john.residence = johnsHouse
 
if let firstRoomName = john.residence?[0].name {
    println("The first room name is \(firstRoomName).")
} else {
    println("Unable to retrieve the first room name.")
}
// prints "The first room name is Living Room."
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
var testScores = ["Dave": [86, 82, 84], "Tim": [79, 94, 81]]
testScores["Dave"]?[0] = 91
testScores["Tim"]?[0]++
testScores["Brian"]?[0] = 72
// the "Dave" array is now [91, 82, 84] and the "Tim" array is now [80, 94, 81]
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
if let johnsStreet = john.residence?.address?.street {
    println("John's street name is \(johnsStreet).")
} else {
    println("Unable to retrieve the address.")
}
// prints "Unable to retrieve the address."
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
let johnsAddress = Address()
johnsAddress.buildingName = "The Larches"
johnsAddress.street = "Laurel Street"
john.residence!.address = johnsAddress
 
if let johnsStreet = john.residence?.address?.street {
    println("John's street name is \(johnsStreet).")
} else {
    println("Unable to retrieve the address.")
}
// prints "John's street name is Laurel Street."
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
if let buildingIdentifier = john.residence?.address?.buildingIdentifier() {
    println("John's building identifier is \(buildingIdentifier).")
}
// prints "John's building identifier is The Larches."
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
if let beginsWithThe =
    john.residence?.address?.buildingIdentifier()?.hasPrefix("The") {
        if beginsWithThe {
            println("John's building identifier begins with \"The\".")
        } else {
            println("John's building identifier does not begin with \"The\".")
        }
}
// prints "John's building identifier begins with "The"."
        """
        this.Test (code)

