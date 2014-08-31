namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type SubscriptsTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
subscript(index: Int) -> Int {
    get {
        // return an appropriate subscript value here
    }
    set(newValue) {
        // perform a suitable setting action here
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
subscript(index: Int) -> Int {
    // return an appropriate subscript value here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
struct TimesTable {
    let multiplier: Int
    subscript(index: Int) -> Int {
        return multiplier * index
    }
}
let threeTimesTable = TimesTable(multiplier: 3)
println("six times three is \(threeTimesTable[6])")
// prints "six times three is 18"
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
var numberOfLegs = ["spider": 8, "ant": 6, "cat": 4]
numberOfLegs["bird"] = 2
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
struct Matrix {
    let rows: Int, columns: Int
    var grid: [Double]
    init(rows: Int, columns: Int) {
        self.rows = rows
        self.columns = columns
        grid = Array(count: rows * columns, repeatedValue: 0.0)
    }
    func indexIsValidForRow(row: Int, column: Int) -> Bool {
        return row >= 0 && row < rows && column >= 0 && column < columns
    }
    subscript(row: Int, column: Int) -> Double {
        get {
            assert(indexIsValidForRow(row, column: column), "Index out of range")
            return grid[(row * columns) + column]
        }
        set {
            assert(indexIsValidForRow(row, column: column), "Index out of range")
            grid[(row * columns) + column] = newValue
        }
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
var matrix = Matrix(rows: 2, columns: 2)
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
matrix[0, 1] = 1.5
matrix[1, 0] = 3.2
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
func indexIsValidForRow(row: Int, column: Int) -> Bool {
    return row >= 0 && row < rows && column >= 0 && column < columns
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
let someValue = matrix[2, 2]
// this triggers an assert, because [2, 2] is outside of the matrix bounds
        """
        this.Test (code)

