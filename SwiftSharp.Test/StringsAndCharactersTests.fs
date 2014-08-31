namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type StringsAndCharactersTests () =
    inherit BookTests ()

    [<Test>]
    member this.StringsAndCharacters01() =
        let code = """let someString = "Some string literal value" """
        this.Test ("StringsAndCharacters01", code)

    [<Test>]
    member this.StringsAndCharacters02() =
        let code = """var emptyString = ""               // empty string literal
var anotherEmptyString = String()  // initializer syntax
// these two strings are both empty, and are equivalent to each other """
        this.Test ("StringsAndCharacters02", code)

    [<Test>]
    member this.StringsAndCharacters03() =
        let code = """if emptyString.isEmpty {
    println("Nothing to see here")
}
// prints "Nothing to see here" """
        this.Test ("StringsAndCharacters03", code)

    [<Test>]
    member this.StringsAndCharacters04() =
        let code = """var variableString = "Horse"
variableString += " and carriage"
// variableString is now "Horse and carriage"
 
let constantString = "Highlander"
constantString += " and another Highlander"
// this reports a compile-time error - a constant string cannot be modified """
        this.Test ("StringsAndCharacters04", code)

    [<Test>]
    member this.StringsAndCharacters05() =
        let code = """for character in "Dog!🐶" {
    println(character)
}
// D
// o
// g
// !
// 🐶 """
        this.Test ("StringsAndCharacters05", code)

    [<Test>]
    member this.StringsAndCharacters06() =
        let code = """let yenSign: Character = "¥" """
        this.Test ("StringsAndCharacters06", code)

    [<Test>]
    member this.StringsAndCharacters07() =
        let code = """let string1 = "hello"
let string2 = " there"
var welcome = string1 + string2
// welcome now equals "hello there" """
        this.Test ("StringsAndCharacters07", code)

    [<Test>]
    member this.StringsAndCharacters08() =
        let code = """var instruction = "look over"
instruction += string2
// instruction now equals "look over there" """
        this.Test ("StringsAndCharacters08", code)

    [<Test>]
    member this.StringsAndCharacters09() =
        let code = """let exclamationMark: Character = "!"
welcome.append(exclamationMark)
// welcome now equals "hello there!" """
        this.Test ("StringsAndCharacters09", code)

    [<Test>]
    member this.StringsAndCharacters10() =
        let code = """let multiplier = 3
let message = "\(multiplier) times 2.5 is \(Double(multiplier) * 2.5)"
// message is "3 times 2.5 is 7.5" """
        this.Test ("StringsAndCharacters10", code)

    [<Test>]
    member this.StringsAndCharacters11() =
        let code = """let wiseWords = "\"Imagination is more important than knowledge\" - Einstein"
// "Imagination is more important than knowledge" - Einstein
let dollarSign = "\u{24}"        // $,  Unicode scalar U+0024
let blackHeart = "\u{2665}"      // ♥,  Unicode scalar U+2665
let sparklingHeart = "\u{1F496}" // 💖, Unicode scalar U+1F496 """
        this.Test ("StringsAndCharacters11", code)

    [<Test>]
    member this.StringsAndCharacters12() =
        let code = """let eAcute: Character = "\u{E9}"                         // é
let combinedEAcute: Character = "\u{65}\u{301}"          // e followed by ́
// eAcute is é, combinedEAcute is é """
        this.Test ("StringsAndCharacters12", code)

    [<Test>]
    member this.StringsAndCharacters13() =
        let code = """let precomposed: Character = "\u{D55C}"                  // 한
let decomposed: Character = "\u{1112}\u{1161}\u{11AB}"   // ᄒ, ᅡ, ᆫ
// precomposed is 한, decomposed is 한 """
        this.Test ("StringsAndCharacters13", code)

    [<Test>]
    member this.StringsAndCharacters14() =
        let code = """let enclosedEAcute: Character = "\u{E9}\u{20DD}"
// enclosedEAcute is é⃝ """
        this.Test ("StringsAndCharacters14", code)

    [<Test>]
    member this.StringsAndCharacters15() =
        let code = """let regionalIndicatorForUS: Character = "\u{1F1FA}\u{1F1F8}"
// regionalIndicatorForUS is 🇺🇸 """
        this.Test ("StringsAndCharacters15", code)

    [<Test>]
    member this.StringsAndCharacters16() =
        let code = """let unusualMenagerie = "Koala 🐨, Snail 🐌, Penguin 🐧, Dromedary 🐪"
println("unusualMenagerie has \(countElements(unusualMenagerie)) characters")
// prints "unusualMenagerie has 40 characters" """
        this.Test ("StringsAndCharacters16", code)

    [<Test>]
    member this.StringsAndCharacters17() =
        let code = """var word = "cafe"
println("the number of characters in \(word) is \(countElements(word))")
// prints "the number of characters in cafe is 4"
 
word += "\u{301}"    // COMBINING ACUTE ACCENT, U+0301
 
println("the number of characters in \(word) is \(countElements(word))")
// prints "the number of characters in café is 4" """
        this.Test ("StringsAndCharacters17", code)

    [<Test>]
    member this.StringsAndCharacters18() =
        let code = """let quotation = "We're a lot alike, you and I."
let sameQuotation = "We're a lot alike, you and I."
if quotation == sameQuotation {
    println("These two strings are considered equal")
}
// prints "These two strings are considered equal" """
        this.Test ("StringsAndCharacters18", code)

    [<Test>]
    member this.StringsAndCharacters19() =
        let code = """// "Voulez-vous un café?" using LATIN SMALL LETTER E WITH ACUTE
let eAcuteQuestion = "Voulez-vous un caf\u{E9}?"
 
// "Voulez-vous un café?" using LATIN SMALL LETTER E and COMBINING ACUTE ACCENT
let combinedEAcuteQuestion = "Voulez-vous un caf\u{65}\u{301}?"
 
if eAcuteQuestion == combinedEAcuteQuestion {
    println("These two strings are considered equal")
}
// prints "These two strings are considered equal" """
        this.Test ("StringsAndCharacters19", code)

    [<Test>]
    member this.StringsAndCharacters20() =
        let code = """let latinCapitalLetterA: Character = "\u{41}"
 
let cyrillicCapitalLetterA: Character = "\u{0410}"
 
if latinCapitalLetterA != cyrillicCapitalLetterA {
    println("These two characters are not equivalent")
}
// prints "These two characters are not equivalent" """
        this.Test ("StringsAndCharacters20", code)

    [<Test>]
    member this.StringsAndCharacters21() =
        let code = """let romeoAndJuliet = [
    "Act 1 Scene 1: Verona, A public place",
    "Act 1 Scene 2: Capulet's mansion",
    "Act 1 Scene 3: A room in Capulet's mansion",
    "Act 1 Scene 4: A street outside Capulet's mansion",
    "Act 1 Scene 5: The Great Hall in Capulet's mansion",
    "Act 2 Scene 1: Outside Capulet's mansion",
    "Act 2 Scene 2: Capulet's orchard",
    "Act 2 Scene 3: Outside Friar Lawrence's cell",
    "Act 2 Scene 4: A street in Verona",
    "Act 2 Scene 5: Capulet's mansion",
    "Act 2 Scene 6: Friar Lawrence's cell"
] """
        this.Test ("StringsAndCharacters21", code)

    [<Test>]
    member this.StringsAndCharacters22() =
        let code = """var act1SceneCount = 0
for scene in romeoAndJuliet {
    if scene.hasPrefix("Act 1 ") {
        ++act1SceneCount
    }
}
println("There are \(act1SceneCount) scenes in Act 1")
// prints "There are 5 scenes in Act 1" """
        this.Test ("StringsAndCharacters22", code)

    [<Test>]
    member this.StringsAndCharacters23() =
        let code = """var mansionCount = 0
var cellCount = 0
for scene in romeoAndJuliet {
    if scene.hasSuffix("Capulet's mansion") {
        ++mansionCount
    } else if scene.hasSuffix("Friar Lawrence's cell") {
        ++cellCount
    }
}
println("\(mansionCount) mansion scenes; \(cellCount) cell scenes")
// prints "6 mansion scenes; 2 cell scenes" """
        this.Test ("StringsAndCharacters23", code)

    [<Test>]
    member this.StringsAndCharacters24() =
        let code = """let dogString = "Dog‼🐶" """
        this.Test ("StringsAndCharacters24", code)

    [<Test>]
    member this.StringsAndCharacters25() =
        let code = """for codeUnit in dogString.utf8 {
    print("\(codeUnit) ")
}
print("\n")
// 68 111 103 226 128 188 240 159 144 182 """
        this.Test ("StringsAndCharacters25", code)

    [<Test>]
    member this.StringsAndCharacters26() =
        let code = """for codeUnit in dogString.utf16 {
    print("\(codeUnit) ")
}
print("\n")
// 68 111 103 8252 55357 56374 """
        this.Test ("StringsAndCharacters26", code)

    [<Test>]
    member this.StringsAndCharacters27() =
        let code = """for scalar in dogString.unicodeScalars {
    print("\(scalar.value) ")
}
print("\n")
// 68 111 103 8252 128054 """
        this.Test ("StringsAndCharacters27", code)

    [<Test>]
    member this.StringsAndCharacters28() =
        let code = """for scalar in dogString.unicodeScalars {
    println("\(scalar) ")
}
// D
// o
// g
// ‼
// 🐶 """
        this.Test ("StringsAndCharacters28", code)

