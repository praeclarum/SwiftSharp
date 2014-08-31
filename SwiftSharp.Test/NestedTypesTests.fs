namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type NestedTypesTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
struct BlackjackCard {
    
    // nested Suit enumeration
    enum Suit: Character {
        case Spades = "♠", Hearts = "♡", Diamonds = "♢", Clubs = "♣"
    }
    
    // nested Rank enumeration
    enum Rank: Int {
        case Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten
        case Jack, Queen, King, Ace
        struct Values {
            let first: Int, second: Int?
        }
        var values: Values {
            switch self {
            case .Ace:
                return Values(first: 1, second: 11)
            case .Jack, .Queen, .King:
                return Values(first: 10, second: nil)
            default:
                return Values(first: self.toRaw(), second: nil)
                }
        }
    }
    
    // BlackjackCard properties and methods
    let rank: Rank, suit: Suit
    var description: String {
        var output = "suit is \(suit.toRaw()),"
            output += " value is \(rank.values.first)"
            if let second = rank.values.second {
                output += " or \(second)"
            }
            return output
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
let theAceOfSpades = BlackjackCard(rank: .Ace, suit: .Spades)
println("theAceOfSpades: \(theAceOfSpades.description)")
// prints "theAceOfSpades: suit is ♠, value is 1 or 11"
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
let heartsSymbol = BlackjackCard.Suit.Hearts.toRaw()
// heartsSymbol is "♡"
        """
        this.Test (code)

