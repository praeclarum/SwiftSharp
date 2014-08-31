namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ProtocolsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
protocol SomeProtocol {
    // protocol definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
struct SomeStructure: FirstProtocol, AnotherProtocol {
    // structure definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
class SomeClass: SomeSuperclass, FirstProtocol, AnotherProtocol {
    // class definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
protocol SomeProtocol {
    var mustBeSettable: Int { get set }
    var doesNotNeedToBeSettable: Int { get }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
protocol AnotherProtocol {
    class var someTypeProperty: Int { get set }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
protocol FullyNamed {
    var fullName: String { get }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
struct Person: FullyNamed {
    var fullName: String
}
let john = Person(fullName: "John Appleseed")
// john.fullName is "John Appleseed"
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
class Starship: FullyNamed {
    var prefix: String?
    var name: String
    init(name: String, prefix: String? = nil) {
        self.name = name
        self.prefix = prefix
    }
    var fullName: String {
        return (prefix != nil ? prefix! + " " : "") + name
    }
}
var ncc1701 = Starship(name: "Enterprise", prefix: "USS")
// ncc1701.fullName is "USS Enterprise"
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
protocol SomeProtocol {
    class func someTypeMethod()
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
protocol RandomNumberGenerator {
    func random() -> Double
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
class LinearCongruentialGenerator: RandomNumberGenerator {
    var lastRandom = 42.0
    let m = 139968.0
    let a = 3877.0
    let c = 29573.0
    func random() -> Double {
        lastRandom = ((lastRandom * a + c) % m)
        return lastRandom / m
    }
}
let generator = LinearCongruentialGenerator()
println("Here's a random number: \(generator.random())")
// prints "Here's a random number: 0.37464991998171"
println("And another one: \(generator.random())")
// prints "And another one: 0.729023776863283"
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
protocol Togglable {
    mutating func toggle()
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
enum OnOffSwitch: Togglable {
    case Off, On
    mutating func toggle() {
        switch self {
        case Off:
            self = On
        case On:
            self = Off
        }
    }
}
var lightSwitch = OnOffSwitch.Off
lightSwitch.toggle()
// lightSwitch is now equal to .On
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
protocol SomeProtocol {
    init(someParameter: Int)
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
class SomeClass: SomeProtocol {
    required init(someParameter: Int) {
        // initializer implementation goes here
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
protocol SomeProtocol {
    init()
}
 
class SomeSuperClass {
    init() {
        // initializer implementation goes here
    }
}
 
class SomeSubClass: SomeSuperClass, SomeProtocol {
    // "required" from SomeProtocol conformance; "override" from SomeSuperClass
    required override init() {
        // initializer implementation goes here
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
class Dice {
    let sides: Int
    let generator: RandomNumberGenerator
    init(sides: Int, generator: RandomNumberGenerator) {
        self.sides = sides
        self.generator = generator
    }
    func roll() -> Int {
        return Int(generator.random() * Double(sides)) + 1
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
var d6 = Dice(sides: 6, generator: LinearCongruentialGenerator())
for _ in 1...5 {
    println("Random dice roll is \(d6.roll())")
}
// Random dice roll is 3
// Random dice roll is 5
// Random dice roll is 4
// Random dice roll is 5
// Random dice roll is 4
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
protocol DiceGame {
    var dice: Dice { get }
    func play()
}
protocol DiceGameDelegate {
    func gameDidStart(game: DiceGame)
    func game(game: DiceGame, didStartNewTurnWithDiceRoll diceRoll: Int)
    func gameDidEnd(game: DiceGame)
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
class SnakesAndLadders: DiceGame {
    let finalSquare = 25
    let dice = Dice(sides: 6, generator: LinearCongruentialGenerator())
    var square = 0
    var board: [Int]
    init() {
        board = [Int](count: finalSquare + 1, repeatedValue: 0)
        board[03] = +08; board[06] = +11; board[09] = +09; board[10] = +02
        board[14] = -10; board[19] = -11; board[22] = -02; board[24] = -08
    }
    var delegate: DiceGameDelegate?
    func play() {
        square = 0
        delegate?.gameDidStart(self)
        gameLoop: while square != finalSquare {
            let diceRoll = dice.roll()
            delegate?.game(self, didStartNewTurnWithDiceRoll: diceRoll)
            switch square + diceRoll {
            case finalSquare:
                break gameLoop
            case let newSquare where newSquare > finalSquare:
                continue gameLoop
            default:
                square += diceRoll
                square += board[square]
            }
        }
        delegate?.gameDidEnd(self)
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
class DiceGameTracker: DiceGameDelegate {
    var numberOfTurns = 0
    func gameDidStart(game: DiceGame) {
        numberOfTurns = 0
        if game is SnakesAndLadders {
            println("Started a new game of Snakes and Ladders")
        }
        println("The game is using a \(game.dice.sides)-sided dice")
    }
    func game(game: DiceGame, didStartNewTurnWithDiceRoll diceRoll: Int) {
        ++numberOfTurns
        println("Rolled a \(diceRoll)")
    }
    func gameDidEnd(game: DiceGame) {
        println("The game lasted for \(numberOfTurns) turns")
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
let tracker = DiceGameTracker()
let game = SnakesAndLadders()
game.delegate = tracker
game.play()
// Started a new game of Snakes and Ladders
// The game is using a 6-sided dice
// Rolled a 3
// Rolled a 5
// Rolled a 4
// Rolled a 5
// The game lasted for 4 turns
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
protocol TextRepresentable {
    func asText() -> String
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
extension Dice: TextRepresentable {
    func asText() -> String {
        return "A \(sides)-sided dice"
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
let d12 = Dice(sides: 12, generator: LinearCongruentialGenerator())
println(d12.asText())
// prints "A 12-sided dice"
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
extension SnakesAndLadders: TextRepresentable {
    func asText() -> String {
        return "A game of Snakes and Ladders with \(finalSquare) squares"
    }
}
println(game.asText())
// prints "A game of Snakes and Ladders with 25 squares"
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
struct Hamster {
    var name: String
    func asText() -> String {
        return "A hamster named \(name)"
    }
}
extension Hamster: TextRepresentable {}
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
let simonTheHamster = Hamster(name: "Simon")
let somethingTextRepresentable: TextRepresentable = simonTheHamster
println(somethingTextRepresentable.asText())
// prints "A hamster named Simon"
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
let things: [TextRepresentable] = [game, d12, simonTheHamster]
        """
        this.Test (code)

    [<Test>]
    member this.Sample30() =
        let code = """
for thing in things {
    println(thing.asText())
}
// A game of Snakes and Ladders with 25 squares
// A 12-sided dice
// A hamster named Simon
        """
        this.Test (code)

    [<Test>]
    member this.Sample31() =
        let code = """
protocol InheritingProtocol: SomeProtocol, AnotherProtocol {
    // protocol definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample32() =
        let code = """
protocol PrettyTextRepresentable: TextRepresentable {
    func asPrettyText() -> String
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample33() =
        let code = """
extension SnakesAndLadders: PrettyTextRepresentable {
    func asPrettyText() -> String {
        var output = asText() + ":\n"
        for index in 1...finalSquare {
            switch board[index] {
            case let ladder where ladder > 0:
                output += "▲ "
            case let snake where snake < 0:
                output += "▼ "
            default:
                output += "○ "
            }
        }
        return output
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample34() =
        let code = """
println(game.asPrettyText())
// A game of Snakes and Ladders with 25 squares:
// ○ ○ ▲ ○ ○ ▲ ○ ○ ▲ ▲ ○ ○ ○ ▼ ○ ○ ○ ○ ▼ ○ ○ ▼ ○ ▼ ○
        """
        this.Test (code)

    [<Test>]
    member this.Sample35() =
        let code = """
protocol SomeClassOnlyProtocol: class, SomeInheritedProtocol {
    // class-only protocol definition goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample36() =
        let code = """
protocol Named {
    var name: String { get }
}
protocol Aged {
    var age: Int { get }
}
struct Person: Named, Aged {
    var name: String
    var age: Int
}
func wishHappyBirthday(celebrator: protocol<Named, Aged>) {
    println("Happy birthday \(celebrator.name) - you're \(celebrator.age)!")
}
let birthdayPerson = Person(name: "Malcolm", age: 21)
wishHappyBirthday(birthdayPerson)
// prints "Happy birthday Malcolm - you're 21!"
        """
        this.Test (code)

    [<Test>]
    member this.Sample37() =
        let code = """
@objc protocol HasArea {
    var area: Double { get }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample38() =
        let code = """
class Circle: HasArea {
    let pi = 3.1415927
    var radius: Double
    var area: Double { return pi * radius * radius }
    init(radius: Double) { self.radius = radius }
}
class Country: HasArea {
    var area: Double
    init(area: Double) { self.area = area }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample39() =
        let code = """
class Animal {
    var legs: Int
    init(legs: Int) { self.legs = legs }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample40() =
        let code = """
let objects: [AnyObject] = [
    Circle(radius: 2.0),
    Country(area: 243_610),
    Animal(legs: 4)
]
        """
        this.Test (code)

    [<Test>]
    member this.Sample41() =
        let code = """
for object in objects {
    if let objectWithArea = object as? HasArea {
        println("Area is \(objectWithArea.area)")
    } else {
        println("Something that doesn't have an area")
    }
}
// Area is 12.5663708
// Area is 243610.0
// Something that doesn't have an area
        """
        this.Test (code)

    [<Test>]
    member this.Sample42() =
        let code = """
@objc protocol CounterDataSource {
    optional func incrementForCount(count: Int) -> Int
    optional var fixedIncrement: Int { get }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample43() =
        let code = """
@objc class Counter {
    var count = 0
    var dataSource: CounterDataSource?
    func increment() {
        if let amount = dataSource?.incrementForCount?(count) {
            count += amount
        } else if let amount = dataSource?.fixedIncrement? {
            count += amount
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample44() =
        let code = """
class ThreeSource: CounterDataSource {
    let fixedIncrement = 3
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample45() =
        let code = """
var counter = Counter()
counter.dataSource = ThreeSource()
for _ in 1...4 {
    counter.increment()
    println(counter.count)
}
// 3
// 6
// 9
// 12
        """
        this.Test (code)

    [<Test>]
    member this.Sample46() =
        let code = """
class TowardsZeroSource: CounterDataSource {
    func incrementForCount(count: Int) -> Int {
        if count == 0 {
            return 0
        } else if count < 0 {
            return 1
        } else {
            return -1
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample47() =
        let code = """
counter.count = -4
counter.dataSource = TowardsZeroSource()
for _ in 1...5 {
    counter.increment()
    println(counter.count)
}
// -3
// -2
// -1
// 0
// 0
        """
        this.Test (code)

