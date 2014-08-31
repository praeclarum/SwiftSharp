namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type OptionalChainingTests () =
    inherit BookTests ()

    [<Test>]
    member this.OptionalChaining01() =
        let code = """class Person {
    var residence: Residence?
}
 
class Residence {
    var numberOfRooms = 1
} """
        this.Test ("OptionalChaining01", code)

    [<Test>]
    member this.OptionalChaining02() =
        let code = """let john = Person() """
        this.Test ("OptionalChaining02", code)

    [<Test>]
    member this.OptionalChaining03() =
        let code = """let roomCount = john.residence!.numberOfRooms
// this triggers a runtime error """
        this.Test ("OptionalChaining03", code)

    [<Test>]
    member this.OptionalChaining04() =
        let code = """if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "Unable to retrieve the number of rooms." """
        this.Test ("OptionalChaining04", code)

    [<Test>]
    member this.OptionalChaining05() =
        let code = """john.residence = Residence() """
        this.Test ("OptionalChaining05", code)

    [<Test>]
    member this.OptionalChaining06() =
        let code = """if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "John's residence has 1 room(s)." """
        this.Test ("OptionalChaining06", code)

    [<Test>]
    member this.OptionalChaining07() =
        let code = """class Person {
    var residence: Residence?
} """
        this.Test ("OptionalChaining07", code)

    [<Test>]
    member this.OptionalChaining08() =
        let code = """class Residence {
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
} """
        this.Test ("OptionalChaining08", code)

    [<Test>]
    member this.OptionalChaining09() =
        let code = """class Room {
    let name: String
    init(name: String) { self.name = name }
} """
        this.Test ("OptionalChaining09", code)

    [<Test>]
    member this.OptionalChaining10() =
        let code = """class Address {
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
} """
        this.Test ("OptionalChaining10", code)

    [<Test>]
    member this.OptionalChaining11() =
        let code = """let john = Person()
if let roomCount = john.residence?.numberOfRooms {
    println("John's residence has \(roomCount) room(s).")
} else {
    println("Unable to retrieve the number of rooms.")
}
// prints "Unable to retrieve the number of rooms." """
        this.Test ("OptionalChaining11", code)

    [<Test>]
    member this.OptionalChaining12() =
        let code = """let someAddress = Address()
someAddress.buildingNumber = "29"
someAddress.street = "Acacia Road"
john.residence?.address = someAddress """
        this.Test ("OptionalChaining12", code)

    [<Test>]
    member this.OptionalChaining13() =
        let code = """func printNumberOfRooms() {
    println("The number of rooms is \(numberOfRooms)")
} """
        this.Test ("OptionalChaining13", code)

    [<Test>]
    member this.OptionalChaining14() =
        let code = """if john.residence?.printNumberOfRooms() != nil {
    println("It was possible to print the number of rooms.")
} else {
    println("It was not possible to print the number of rooms.")
}
// prints "It was not possible to print the number of rooms." """
        this.Test ("OptionalChaining14", code)

    [<Test>]
    member this.OptionalChaining15() =
        let code = """if (john.residence?.address = someAddress) != nil {
    println("It was possible to set the address.")
} else {
    println("It was not possible to set the address.")
}
// prints "It was not possible to set the address." """
        this.Test ("OptionalChaining15", code)

    [<Test>]
    member this.OptionalChaining16() =
        let code = """if let firstRoomName = john.residence?[0].name {
    println("The first room name is \(firstRoomName).")
} else {
    println("Unable to retrieve the first room name.")
}
// prints "Unable to retrieve the first room name." """
        this.Test ("OptionalChaining16", code)

    [<Test>]
    member this.OptionalChaining17() =
        let code = """john.residence?[0] = Room(name: "Bathroom") """
        this.Test ("OptionalChaining17", code)

    [<Test>]
    member this.OptionalChaining18() =
        let code = """let johnsHouse = Residence()
johnsHouse.rooms.append(Room(name: "Living Room"))
johnsHouse.rooms.append(Room(name: "Kitchen"))
john.residence = johnsHouse
 
if let firstRoomName = john.residence?[0].name {
    println("The first room name is \(firstRoomName).")
} else {
    println("Unable to retrieve the first room name.")
}
// prints "The first room name is Living Room." """
        this.Test ("OptionalChaining18", code)

    [<Test>]
    member this.OptionalChaining19() =
        let code = """var testScores = ["Dave": [86, 82, 84], "Tim": [79, 94, 81]]
testScores["Dave"]?[0] = 91
testScores["Tim"]?[0]++
testScores["Brian"]?[0] = 72
// the "Dave" array is now [91, 82, 84] and the "Tim" array is now [80, 94, 81] """
        this.Test ("OptionalChaining19", code)

    [<Test>]
    member this.OptionalChaining20() =
        let code = """if let johnsStreet = john.residence?.address?.street {
    println("John's street name is \(johnsStreet).")
} else {
    println("Unable to retrieve the address.")
}
// prints "Unable to retrieve the address." """
        this.Test ("OptionalChaining20", code)

    [<Test>]
    member this.OptionalChaining21() =
        let code = """let johnsAddress = Address()
johnsAddress.buildingName = "The Larches"
johnsAddress.street = "Laurel Street"
john.residence!.address = johnsAddress
 
if let johnsStreet = john.residence?.address?.street {
    println("John's street name is \(johnsStreet).")
} else {
    println("Unable to retrieve the address.")
}
// prints "John's street name is Laurel Street." """
        this.Test ("OptionalChaining21", code)

    [<Test>]
    member this.OptionalChaining22() =
        let code = """if let buildingIdentifier = john.residence?.address?.buildingIdentifier() {
    println("John's building identifier is \(buildingIdentifier).")
}
// prints "John's building identifier is The Larches." """
        this.Test ("OptionalChaining22", code)

    [<Test>]
    member this.OptionalChaining23() =
        let code = """if let beginsWithThe =
    john.residence?.address?.buildingIdentifier()?.hasPrefix("The") {
        if beginsWithThe {
            println("John's building identifier begins with \"The\".")
        } else {
            println("John's building identifier does not begin with \"The\".")
        }
}
// prints "John's building identifier begins with "The"." """
        this.Test ("OptionalChaining23", code)

