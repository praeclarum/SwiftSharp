namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type ASwiftTourTests () =
    inherit BookTests ()

    [<Test>]
    member this.ASwiftTour01() =
        let code = """println("Hello, world!") """
        this.Test ("ASwiftTour01", code)

    [<Test>]
    member this.ASwiftTour02() =
        let code = """var myVariable = 42
myVariable = 50
let myConstant = 42 """
        this.Test ("ASwiftTour02", code)

    [<Test>]
    member this.ASwiftTour03() =
        let code = """let implicitInteger = 70
let implicitDouble = 70.0
let explicitDouble: Double = 70 """
        this.Test ("ASwiftTour03", code)

    [<Test>]
    member this.ASwiftTour04() =
        let code = """let label = "The width is "
let width = 94
let widthLabel = label + String(width) """
        this.Test ("ASwiftTour04", code)

    [<Test>]
    member this.ASwiftTour05() =
        let code = """let apples = 3
let oranges = 5
let appleSummary = "I have \(apples) apples."
let fruitSummary = "I have \(apples + oranges) pieces of fruit." """
        this.Test ("ASwiftTour05", code)

    [<Test>]
    member this.ASwiftTour06() =
        let code = """var shoppingList = ["catfish", "water", "tulips", "blue paint"]
shoppingList[1] = "bottle of water"
 
var occupations = [
    "Malcolm": "Captain",
    "Kaylee": "Mechanic",
]
occupations["Jayne"] = "Public Relations" """
        this.Test ("ASwiftTour06", code)

    [<Test>]
    member this.ASwiftTour07() =
        let code = """let emptyArray = [String]()
let emptyDictionary = [String: Float]() """
        this.Test ("ASwiftTour07", code)

    [<Test>]
    member this.ASwiftTour08() =
        let code = """shoppingList = []
occupations = [:] """
        this.Test ("ASwiftTour08", code)

    [<Test>]
    member this.ASwiftTour09() =
        let code = """let individualScores = [75, 43, 103, 87, 12]
var teamScore = 0
for score in individualScores {
    if score > 50 {
        teamScore += 3
    } else {
        teamScore += 1
    }
}
teamScore """
        this.Test ("ASwiftTour09", code)

    [<Test>]
    member this.ASwiftTour10() =
        let code = """var optionalString: String? = "Hello"
optionalString == nil
 
var optionalName: String? = "John Appleseed"
var greeting = "Hello!"
if let name = optionalName {
    greeting = "Hello, \(name)"
} """
        this.Test ("ASwiftTour10", code)

    [<Test>]
    member this.ASwiftTour11() =
        let code = """let vegetable = "red pepper"
switch vegetable {
case "celery":
    let vegetableComment = "Add some raisins and make ants on a log."
case "cucumber", "watercress":
    let vegetableComment = "That would make a good tea sandwich."
case let x where x.hasSuffix("pepper"):
    let vegetableComment = "Is it a spicy \(x)?"
default:
    let vegetableComment = "Everything tastes good in soup."
} """
        this.Test ("ASwiftTour11", code)

    [<Test>]
    member this.ASwiftTour12() =
        let code = """let interestingNumbers = [
    "Prime": [2, 3, 5, 7, 11, 13],
    "Fibonacci": [1, 1, 2, 3, 5, 8],
    "Square": [1, 4, 9, 16, 25],
]
var largest = 0
for (kind, numbers) in interestingNumbers {
    for number in numbers {
        if number > largest {
            largest = number
        }
    }
}
largest """
        this.Test ("ASwiftTour12", code)

    [<Test>]
    member this.ASwiftTour13() =
        let code = """var n = 2
while n < 100 {
    n = n * 2
}
n
 
var m = 2
do {
    m = m * 2
} while m < 100
m """
        this.Test ("ASwiftTour13", code)

    [<Test>]
    member this.ASwiftTour14() =
        let code = """var firstForLoop = 0
for i in 0..<4 {
    firstForLoop += i
}
firstForLoop
 
var secondForLoop = 0
for var i = 0; i < 4; ++i {
    secondForLoop += i
}
secondForLoop """
        this.Test ("ASwiftTour14", code)

    [<Test>]
    member this.ASwiftTour15() =
        let code = """func greet(name: String, day: String) -> String {
    return "Hello \(name), today is \(day)."
}
greet("Bob", "Tuesday") """
        this.Test ("ASwiftTour15", code)

    [<Test>]
    member this.ASwiftTour16() =
        let code = """func calculateStatistics(scores: [Int]) -> (min: Int, max: Int, sum: Int) {
    var min = scores[0]
    var max = scores[0]
    var sum = 0
    
    for score in scores {
        if score > max {
            max = score
        } else if score < min {
            min = score
        }
        sum += score
    }
    
    return (min, max, sum)
}
let statistics = calculateStatistics([5, 3, 100, 3, 9])
statistics.sum
statistics.2 """
        this.Test ("ASwiftTour16", code)

    [<Test>]
    member this.ASwiftTour17() =
        let code = """func sumOf(numbers: Int...) -> Int {
    var sum = 0
    for number in numbers {
        sum += number
    }
    return sum
}
sumOf()
sumOf(42, 597, 12) """
        this.Test ("ASwiftTour17", code)

    [<Test>]
    member this.ASwiftTour18() =
        let code = """func returnFifteen() -> Int {
    var y = 10
    func add() {
        y += 5
    }
    add()
    return y
}
returnFifteen() """
        this.Test ("ASwiftTour18", code)

    [<Test>]
    member this.ASwiftTour19() =
        let code = """func makeIncrementer() -> (Int -> Int) {
    func addOne(number: Int) -> Int {
        return 1 + number
    }
    return addOne
}
var increment = makeIncrementer()
increment(7) """
        this.Test ("ASwiftTour19", code)

    [<Test>]
    member this.ASwiftTour20() =
        let code = """func hasAnyMatches(list: [Int], condition: Int -> Bool) -> Bool {
    for item in list {
        if condition(item) {
            return true
        }
    }
    return false
}
func lessThanTen(number: Int) -> Bool {
    return number < 10
}
var numbers = [20, 19, 7, 12]
hasAnyMatches(numbers, lessThanTen) """
        this.Test ("ASwiftTour20", code)

    [<Test>]
    member this.ASwiftTour21() =
        let code = """numbers.map({
    (number: Int) -> Int in
    let result = 3 * number
    return result
}) """
        this.Test ("ASwiftTour21", code)

    [<Test>]
    member this.ASwiftTour22() =
        let code = """let mappedNumbers = numbers.map({ number in 3 * number })
mappedNumbers """
        this.Test ("ASwiftTour22", code)

    [<Test>]
    member this.ASwiftTour23() =
        let code = """let sortedNumbers = sorted(numbers) { $0 > $1 }
sortedNumbers """
        this.Test ("ASwiftTour23", code)

    [<Test>]
    member this.ASwiftTour24() =
        let code = """class Shape {
    var numberOfSides = 0
    func simpleDescription() -> String {
        return "A shape with \(numberOfSides) sides."
    }
} """
        this.Test ("ASwiftTour24", code)

    [<Test>]
    member this.ASwiftTour25() =
        let code = """var shape = Shape()
shape.numberOfSides = 7
var shapeDescription = shape.simpleDescription() """
        this.Test ("ASwiftTour25", code)

    [<Test>]
    member this.ASwiftTour26() =
        let code = """class NamedShape {
    var numberOfSides: Int = 0
    var name: String
    
    init(name: String) {
        self.name = name
    }
    
    func simpleDescription() -> String {
        return "A shape with \(numberOfSides) sides."
    }
} """
        this.Test ("ASwiftTour26", code)

    [<Test>]
    member this.ASwiftTour27() =
        let code = """class Square: NamedShape {
    var sideLength: Double
    
    init(sideLength: Double, name: String) {
        self.sideLength = sideLength
        super.init(name: name)
        numberOfSides = 4
    }
    
    func area() ->  Double {
        return sideLength * sideLength
    }
    
    override func simpleDescription() -> String {
        return "A square with sides of length \(sideLength)."
    }
}
let test = Square(sideLength: 5.2, name: "my test square")
test.area()
test.simpleDescription() """
        this.Test ("ASwiftTour27", code)

    [<Test>]
    member this.ASwiftTour28() =
        let code = """class EquilateralTriangle: NamedShape {
    var sideLength: Double = 0.0
    
    init(sideLength: Double, name: String) {
        self.sideLength = sideLength
        super.init(name: name)
        numberOfSides = 3
    }
    
    var perimeter: Double {
        get {
            return 3.0 * sideLength
        }
        set {
            sideLength = newValue / 3.0
        }
    }
    
    override func simpleDescription() -> String {
        return "An equilateral triangle with sides of length \(sideLength)."
    }
}
var triangle = EquilateralTriangle(sideLength: 3.1, name: "a triangle")
triangle.perimeter
triangle.perimeter = 9.9
triangle.sideLength """
        this.Test ("ASwiftTour28", code)

    [<Test>]
    member this.ASwiftTour29() =
        let code = """class TriangleAndSquare {
    var triangle: EquilateralTriangle {
        willSet {
            square.sideLength = newValue.sideLength
        }
    }
    var square: Square {
        willSet {
            triangle.sideLength = newValue.sideLength
        }
    }
    init(size: Double, name: String) {
        square = Square(sideLength: size, name: name)
        triangle = EquilateralTriangle(sideLength: size, name: name)
    }
}
var triangleAndSquare = TriangleAndSquare(size: 10, name: "another test shape")
triangleAndSquare.square.sideLength
triangleAndSquare.triangle.sideLength
triangleAndSquare.square = Square(sideLength: 50, name: "larger square")
triangleAndSquare.triangle.sideLength """
        this.Test ("ASwiftTour29", code)

    [<Test>]
    member this.ASwiftTour30() =
        let code = """class Counter {
    var count: Int = 0
    func incrementBy(amount: Int, numberOfTimes times: Int) {
        count += amount * times
    }
}
var counter = Counter()
counter.incrementBy(2, numberOfTimes: 7) """
        this.Test ("ASwiftTour30", code)

    [<Test>]
    member this.ASwiftTour31() =
        let code = """let optionalSquare: Square? = Square(sideLength: 2.5, name: "optional square")
let sideLength = optionalSquare?.sideLength """
        this.Test ("ASwiftTour31", code)

    [<Test>]
    member this.ASwiftTour32() =
        let code = """enum Rank: Int {
    case Ace = 1
    case Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten
    case Jack, Queen, King
    func simpleDescription() -> String {
        switch self {
        case .Ace:
            return "ace"
        case .Jack:
            return "jack"
        case .Queen:
            return "queen"
        case .King:
            return "king"
        default:
            return String(self.toRaw())
        }
    }
}
let ace = Rank.Ace
let aceRawValue = ace.toRaw() """
        this.Test ("ASwiftTour32", code)

    [<Test>]
    member this.ASwiftTour33() =
        let code = """if let convertedRank = Rank.fromRaw(3) {
    let threeDescription = convertedRank.simpleDescription()
} """
        this.Test ("ASwiftTour33", code)

    [<Test>]
    member this.ASwiftTour34() =
        let code = """enum Suit {
    case Spades, Hearts, Diamonds, Clubs
    func simpleDescription() -> String {
        switch self {
        case .Spades:
            return "spades"
        case .Hearts:
            return "hearts"
        case .Diamonds:
            return "diamonds"
        case .Clubs:
            return "clubs"
        }
    }
}
let hearts = Suit.Hearts
let heartsDescription = hearts.simpleDescription() """
        this.Test ("ASwiftTour34", code)

    [<Test>]
    member this.ASwiftTour35() =
        let code = """struct Card {
    var rank: Rank
    var suit: Suit
    func simpleDescription() -> String {
        return "The \(rank.simpleDescription()) of \(suit.simpleDescription())"
    }
}
let threeOfSpades = Card(rank: .Three, suit: .Spades)
let threeOfSpadesDescription = threeOfSpades.simpleDescription() """
        this.Test ("ASwiftTour35", code)

    [<Test>]
    member this.ASwiftTour36() =
        let code = """enum ServerResponse {
    case Result(String, String)
    case Error(String)
}
 
let success = ServerResponse.Result("6:00 am", "8:09 pm")
let failure = ServerResponse.Error("Out of cheese.")
 
switch success {
case let .Result(sunrise, sunset):
    let serverResponse = "Sunrise is at \(sunrise) and sunset is at \(sunset)."
case let .Error(error):
    let serverResponse = "Failure...  \(error)"
} """
        this.Test ("ASwiftTour36", code)

    [<Test>]
    member this.ASwiftTour37() =
        let code = """protocol ExampleProtocol {
    var simpleDescription: String { get }
    mutating func adjust()
} """
        this.Test ("ASwiftTour37", code)

    [<Test>]
    member this.ASwiftTour38() =
        let code = """class SimpleClass: ExampleProtocol {
    var simpleDescription: String = "A very simple class."
    var anotherProperty: Int = 69105
    func adjust() {
        simpleDescription += "  Now 100% adjusted."
    }
}
var a = SimpleClass()
a.adjust()
let aDescription = a.simpleDescription
 
struct SimpleStructure: ExampleProtocol {
    var simpleDescription: String = "A simple structure"
    mutating func adjust() {
        simpleDescription += " (adjusted)"
    }
}
var b = SimpleStructure()
b.adjust()
let bDescription = b.simpleDescription """
        this.Test ("ASwiftTour38", code)

    [<Test>]
    member this.ASwiftTour39() =
        let code = """extension Int: ExampleProtocol {
    var simpleDescription: String {
        return "The number \(self)"
    }
    mutating func adjust() {
        self += 42
    }
 }
7.simpleDescription """
        this.Test ("ASwiftTour39", code)

    [<Test>]
    member this.ASwiftTour40() =
        let code = """let protocolValue: ExampleProtocol = a
protocolValue.simpleDescription
// protocolValue.anotherProperty  // Uncomment to see the error """
        this.Test ("ASwiftTour40", code)

    [<Test>]
    member this.ASwiftTour41() =
        let code = """func repeat<ItemType>(item: ItemType, times: Int) -> [ItemType] {
    var result = [ItemType]()
    for i in 0..<times {
        result.append(item)
    }
    return result
}
repeat("knock", 4) """
        this.Test ("ASwiftTour41", code)

    [<Test>]
    member this.ASwiftTour42() =
        let code = """// Reimplement the Swift standard library's optional type
enum OptionalValue<T> {
    case None
    case Some(T)
}
var possibleInteger: OptionalValue<Int> = .None
possibleInteger = .Some(100) """
        this.Test ("ASwiftTour42", code)

    [<Test>]
    member this.ASwiftTour43() =
        let code = """func anyCommonElements <T, U where T: SequenceType, U: SequenceType, T.Generator.Element: Equatable, T.Generator.Element == U.Generator.Element> (lhs: T, rhs: U) -> Bool {
    for lhsItem in lhs {
        for rhsItem in rhs {
            if lhsItem == rhsItem {
                return true
            }
        }
    }
    return false
}
anyCommonElements([1, 2, 3], [3]) """
        this.Test ("ASwiftTour43", code)

