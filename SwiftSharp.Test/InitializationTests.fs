namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type InitializationTests () =
    inherit BookTests ()

    [<Test>]
    member this.Initialization01() =
        let code = """init() {
    // perform some initialization here
} """
        this.Test ("Initialization01", code)

    [<Test>]
    member this.Initialization02() =
        let code = """struct Fahrenheit {
    var temperature: Double
    init() {
        temperature = 32.0
    }
}
var f = Fahrenheit()
println("The default temperature is \(f.temperature)° Fahrenheit")
// prints "The default temperature is 32.0° Fahrenheit" """
        this.Test ("Initialization02", code)

    [<Test>]
    member this.Initialization03() =
        let code = """struct Fahrenheit {
    var temperature = 32.0
} """
        this.Test ("Initialization03", code)

    [<Test>]
    member this.Initialization04() =
        let code = """struct Celsius {
    var temperatureInCelsius: Double
    init(fromFahrenheit fahrenheit: Double) {
        temperatureInCelsius = (fahrenheit - 32.0) / 1.8
    }
    init(fromKelvin kelvin: Double) {
        temperatureInCelsius = kelvin - 273.15
    }
}
let boilingPointOfWater = Celsius(fromFahrenheit: 212.0)
// boilingPointOfWater.temperatureInCelsius is 100.0
let freezingPointOfWater = Celsius(fromKelvin: 273.15)
// freezingPointOfWater.temperatureInCelsius is 0.0 """
        this.Test ("Initialization04", code)

    [<Test>]
    member this.Initialization05() =
        let code = """struct Color {
    let red, green, blue: Double
    init(red: Double, green: Double, blue: Double) {
        self.red   = red
        self.green = green
        self.blue  = blue
    }
    init(white: Double) {
        red   = white
        green = white
        blue  = white
    }
} """
        this.Test ("Initialization05", code)

    [<Test>]
    member this.Initialization06() =
        let code = """let magenta = Color(red: 1.0, green: 0.0, blue: 1.0)
let halfGray = Color(white: 0.5) """
        this.Test ("Initialization06", code)

    [<Test>]
    member this.Initialization07() =
        let code = """let veryGreen = Color(0.0, 1.0, 0.0)
// this reports a compile-time error - external names are required """
        this.Test ("Initialization07", code)

    [<Test>]
    member this.Initialization08() =
        let code = """struct Celsius {
    var temperatureInCelsius: Double
    init(fromFahrenheit fahrenheit: Double) {
        temperatureInCelsius = (fahrenheit - 32.0) / 1.8
    }
    init(fromKelvin kelvin: Double) {
        temperatureInCelsius = kelvin - 273.15
    }
    init(_ celsius: Double) {
        temperatureInCelsius = celsius
    }
}
let bodyTemperature = Celsius(37.0)
// bodyTemperature.temperatureInCelsius is 37.0 """
        this.Test ("Initialization08", code)

    [<Test>]
    member this.Initialization09() =
        let code = """class SurveyQuestion {
    var text: String
    var response: String?
    init(text: String) {
        self.text = text
    }
    func ask() {
        println(text)
    }
}
let cheeseQuestion = SurveyQuestion(text: "Do you like cheese?")
cheeseQuestion.ask()
// prints "Do you like cheese?"
cheeseQuestion.response = "Yes, I do like cheese." """
        this.Test ("Initialization09", code)

    [<Test>]
    member this.Initialization10() =
        let code = """class SurveyQuestion {
    let text: String
    var response: String?
    init(text: String) {
        self.text = text
    }
    func ask() {
        println(text)
    }
}
let beetsQuestion = SurveyQuestion(text: "How about beets?")
beetsQuestion.ask()
// prints "How about beets?"
beetsQuestion.response = "I also like beets. (But not with cheese.)" """
        this.Test ("Initialization10", code)

    [<Test>]
    member this.Initialization11() =
        let code = """class ShoppingListItem {
    var name: String?
    var quantity = 1
    var purchased = false
}
var item = ShoppingListItem() """
        this.Test ("Initialization11", code)

    [<Test>]
    member this.Initialization12() =
        let code = """struct Size {
    var width = 0.0, height = 0.0
}
let twoByTwo = Size(width: 2.0, height: 2.0) """
        this.Test ("Initialization12", code)

    [<Test>]
    member this.Initialization13() =
        let code = """struct Size {
    var width = 0.0, height = 0.0
}
struct Point {
    var x = 0.0, y = 0.0
} """
        this.Test ("Initialization13", code)

    [<Test>]
    member this.Initialization14() =
        let code = """struct Rect {
    var origin = Point()
    var size = Size()
    init() {}
    init(origin: Point, size: Size) {
        self.origin = origin
        self.size = size
    }
    init(center: Point, size: Size) {
        let originX = center.x - (size.width / 2)
        let originY = center.y - (size.height / 2)
        self.init(origin: Point(x: originX, y: originY), size: size)
    }
} """
        this.Test ("Initialization14", code)

    [<Test>]
    member this.Initialization15() =
        let code = """let basicRect = Rect()
// basicRect's origin is (0.0, 0.0) and its size is (0.0, 0.0) """
        this.Test ("Initialization15", code)

    [<Test>]
    member this.Initialization16() =
        let code = """let originRect = Rect(origin: Point(x: 2.0, y: 2.0),
    size: Size(width: 5.0, height: 5.0))
// originRect's origin is (2.0, 2.0) and its size is (5.0, 5.0) """
        this.Test ("Initialization16", code)

    [<Test>]
    member this.Initialization17() =
        let code = """let centerRect = Rect(center: Point(x: 4.0, y: 4.0),
    size: Size(width: 3.0, height: 3.0))
// centerRect's origin is (2.5, 2.5) and its size is (3.0, 3.0) """
        this.Test ("Initialization17", code)

    [<Test>]
    member this.Initialization18() =
        let code = """class Vehicle {
    var numberOfWheels = 0
    var description: String {
        return "\(numberOfWheels) wheel(s)"
    }
} """
        this.Test ("Initialization18", code)

    [<Test>]
    member this.Initialization19() =
        let code = """let vehicle = Vehicle()
println("Vehicle: \(vehicle.description)")
// Vehicle: 0 wheel(s) """
        this.Test ("Initialization19", code)

    [<Test>]
    member this.Initialization20() =
        let code = """class Bicycle: Vehicle {
    override init() {
        super.init()
        numberOfWheels = 2
    }
} """
        this.Test ("Initialization20", code)

    [<Test>]
    member this.Initialization21() =
        let code = """let bicycle = Bicycle()
println("Bicycle: \(bicycle.description)")
// Bicycle: 2 wheel(s) """
        this.Test ("Initialization21", code)

    [<Test>]
    member this.Initialization22() =
        let code = """class Food {
    var name: String
    init(name: String) {
        self.name = name
    }
    convenience init() {
        self.init(name: "[Unnamed]")
    }
} """
        this.Test ("Initialization22", code)

    [<Test>]
    member this.Initialization23() =
        let code = """let namedMeat = Food(name: "Bacon")
// namedMeat's name is "Bacon" """
        this.Test ("Initialization23", code)

    [<Test>]
    member this.Initialization24() =
        let code = """let mysteryMeat = Food()
// mysteryMeat's name is "[Unnamed]" """
        this.Test ("Initialization24", code)

    [<Test>]
    member this.Initialization25() =
        let code = """class RecipeIngredient: Food {
    var quantity: Int
    init(name: String, quantity: Int) {
        self.quantity = quantity
        super.init(name: name)
    }
    override convenience init(name: String) {
        self.init(name: name, quantity: 1)
    }
} """
        this.Test ("Initialization25", code)

    [<Test>]
    member this.Initialization26() =
        let code = """let oneMysteryItem = RecipeIngredient()
let oneBacon = RecipeIngredient(name: "Bacon")
let sixEggs = RecipeIngredient(name: "Eggs", quantity: 6) """
        this.Test ("Initialization26", code)

    [<Test>]
    member this.Initialization27() =
        let code = """class ShoppingListItem: RecipeIngredient {
    var purchased = false
    var description: String {
        var output = "\(quantity) x \(name)"
            output += purchased ? " ✔" : " ✘"
            return output
    }
} """
        this.Test ("Initialization27", code)

    [<Test>]
    member this.Initialization28() =
        let code = """var breakfastList = [
    ShoppingListItem(),
    ShoppingListItem(name: "Bacon"),
    ShoppingListItem(name: "Eggs", quantity: 6),
]
breakfastList[0].name = "Orange juice"
breakfastList[0].purchased = true
for item in breakfastList {
    println(item.description)
}
// 1 x Orange juice ✔
// 1 x Bacon ✘
// 6 x Eggs ✘ """
        this.Test ("Initialization28", code)

    [<Test>]
    member this.Initialization29() =
        let code = """class SomeClass {
    required init() {
        // initializer implementation goes here
    }
} """
        this.Test ("Initialization29", code)

    [<Test>]
    member this.Initialization30() =
        let code = """class SomeSubclass: SomeClass {
    required init() {
        // subclass implementation of the required initializer goes here
    }
} """
        this.Test ("Initialization30", code)

    [<Test>]
    member this.Initialization31() =
        let code = """class SomeClass {
    let someProperty: SomeType = {
        // create a default value for someProperty inside this closure
        // someValue must be of the same type as SomeType
        return someValue
        }()
} """
        this.Test ("Initialization31", code)

    [<Test>]
    member this.Initialization32() =
        let code = """struct Checkerboard {
    let boardColors: [Bool] = {
        var temporaryBoard = [Bool]()
        var isBlack = false
        for i in 1...10 {
            for j in 1...10 {
                temporaryBoard.append(isBlack)
                isBlack = !isBlack
            }
            isBlack = !isBlack
        }
        return temporaryBoard
        }()
    func squareIsBlackAtRow(row: Int, column: Int) -> Bool {
        return boardColors[(row * 10) + column]
    }
} """
        this.Test ("Initialization32", code)

    [<Test>]
    member this.Initialization33() =
        let code = """let board = Checkerboard()
println(board.squareIsBlackAtRow(0, column: 1))
// prints "true"
println(board.squareIsBlackAtRow(9, column: 9))
// prints "false" """
        this.Test ("Initialization33", code)

