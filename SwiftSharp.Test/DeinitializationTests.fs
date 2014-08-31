namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type DeinitializationTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
deinit {
    // perform the deinitialization
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
struct Bank {
    static var coinsInBank = 10_000
    static func vendCoins(var numberOfCoinsToVend: Int) -> Int {
        numberOfCoinsToVend = min(numberOfCoinsToVend, coinsInBank)
        coinsInBank -= numberOfCoinsToVend
        return numberOfCoinsToVend
    }
    static func receiveCoins(coins: Int) {
        coinsInBank += coins
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
class Player {
    var coinsInPurse: Int
    init(coins: Int) {
        coinsInPurse = Bank.vendCoins(coins)
    }
    func winCoins(coins: Int) {
        coinsInPurse += Bank.vendCoins(coins)
    }
    deinit {
        Bank.receiveCoins(coinsInPurse)
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
var playerOne: Player? = Player(coins: 100)
println("A new player has joined the game with \(playerOne!.coinsInPurse) coins")
// prints "A new player has joined the game with 100 coins"
println("There are now \(Bank.coinsInBank) coins left in the bank")
// prints "There are now 9900 coins left in the bank"
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
playerOne!.winCoins(2_000)
println("PlayerOne won 2000 coins & now has \(playerOne!.coinsInPurse) coins")
// prints "PlayerOne won 2000 coins & now has 2100 coins"
println("The bank now only has \(Bank.coinsInBank) coins left")
// prints "The bank now only has 7900 coins left"
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
playerOne = nil
println("PlayerOne has left the game")
// prints "PlayerOne has left the game"
println("The bank now has \(Bank.coinsInBank) coins")
// prints "The bank now has 10000 coins"
        """
        this.Test (code)

