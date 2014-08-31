namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type MethodsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Methods01() =
        let code = """class Counter {
    var count = 0
    func increment() {
        count++
    }
    func incrementBy(amount: Int) {
        count += amount
    }
    func reset() {
        count = 0
    }
} """
        this.Test ("Methods01", code)

    [<Test>]
    member this.Methods02() =
        let code = """let counter = Counter()
// the initial counter value is 0
counter.increment()
// the counter's value is now 1
counter.incrementBy(5)
// the counter's value is now 6
counter.reset()
// the counter's value is now 0 """
        this.Test ("Methods02", code)

    [<Test>]
    member this.Methods03() =
        let code = """class Counter {
    var count: Int = 0
    func incrementBy(amount: Int, numberOfTimes: Int) {
        count += amount * numberOfTimes
    }
} """
        this.Test ("Methods03", code)

    [<Test>]
    member this.Methods04() =
        let code = """let counter = Counter()
counter.incrementBy(5, numberOfTimes: 3)
// counter value is now 15 """
        this.Test ("Methods04", code)

    [<Test>]
    member this.Methods05() =
        let code = """func incrementBy(amount: Int, #numberOfTimes: Int) {
    count += amount * numberOfTimes
} """
        this.Test ("Methods05", code)

    [<Test>]
    member this.Methods06() =
        let code = """func increment() {
    self.count++
} """
        this.Test ("Methods06", code)

    [<Test>]
    member this.Methods07() =
        let code = """struct Point {
    var x = 0.0, y = 0.0
    func isToTheRightOfX(x: Double) -> Bool {
        return self.x > x
    }
}
let somePoint = Point(x: 4.0, y: 5.0)
if somePoint.isToTheRightOfX(1.0) {
    println("This point is to the right of the line where x == 1.0")
}
// prints "This point is to the right of the line where x == 1.0" """
        this.Test ("Methods07", code)

    [<Test>]
    member this.Methods08() =
        let code = """struct Point {
    var x = 0.0, y = 0.0
    mutating func moveByX(deltaX: Double, y deltaY: Double) {
        x += deltaX
        y += deltaY
    }
}
var somePoint = Point(x: 1.0, y: 1.0)
somePoint.moveByX(2.0, y: 3.0)
println("The point is now at (\(somePoint.x), \(somePoint.y))")
// prints "The point is now at (3.0, 4.0)" """
        this.Test ("Methods08", code)

    [<Test>]
    member this.Methods09() =
        let code = """let fixedPoint = Point(x: 3.0, y: 3.0)
fixedPoint.moveByX(2.0, y: 3.0)
// this will report an error """
        this.Test ("Methods09", code)

    [<Test>]
    member this.Methods10() =
        let code = """struct Point {
    var x = 0.0, y = 0.0
    mutating func moveByX(deltaX: Double, y deltaY: Double) {
        self = Point(x: x + deltaX, y: y + deltaY)
    }
} """
        this.Test ("Methods10", code)

    [<Test>]
    member this.Methods11() =
        let code = """enum TriStateSwitch {
    case Off, Low, High
    mutating func next() {
        switch self {
        case Off:
            self = Low
        case Low:
            self = High
        case High:
            self = Off
        }
    }
}
var ovenLight = TriStateSwitch.Low
ovenLight.next()
// ovenLight is now equal to .High
ovenLight.next()
// ovenLight is now equal to .Off """
        this.Test ("Methods11", code)

    [<Test>]
    member this.Methods12() =
        let code = """class SomeClass {
    class func someTypeMethod() {
        // type method implementation goes here
    }
}
SomeClass.someTypeMethod() """
        this.Test ("Methods12", code)

    [<Test>]
    member this.Methods13() =
        let code = """struct LevelTracker {
    static var highestUnlockedLevel = 1
    static func unlockLevel(level: Int) {
        if level > highestUnlockedLevel { highestUnlockedLevel = level }
    }
    static func levelIsUnlocked(level: Int) -> Bool {
        return level <= highestUnlockedLevel
    }
    var currentLevel = 1
    mutating func advanceToLevel(level: Int) -> Bool {
        if LevelTracker.levelIsUnlocked(level) {
            currentLevel = level
            return true
        } else {
            return false
        }
    }
} """
        this.Test ("Methods13", code)

    [<Test>]
    member this.Methods14() =
        let code = """class Player {
    var tracker = LevelTracker()
    let playerName: String
    func completedLevel(level: Int) {
        LevelTracker.unlockLevel(level + 1)
        tracker.advanceToLevel(level + 1)
    }
    init(name: String) {
        playerName = name
    }
} """
        this.Test ("Methods14", code)

    [<Test>]
    member this.Methods15() =
        let code = """var player = Player(name: "Argyrios")
player.completedLevel(1)
println("highest unlocked level is now \(LevelTracker.highestUnlockedLevel)")
// prints "highest unlocked level is now 2" """
        this.Test ("Methods15", code)

    [<Test>]
    member this.Methods16() =
        let code = """player = Player(name: "Beto")
if player.tracker.advanceToLevel(6) {
    println("player is now on level 6")
} else {
    println("level 6 has not yet been unlocked")
}
// prints "level 6 has not yet been unlocked" """
        this.Test ("Methods16", code)

