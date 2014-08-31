namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type TypeCastingTests () =
    inherit BookTests ()

    [<Test>]
    member this.TypeCasting01() =
        let code = """class MediaItem {
    var name: String
    init(name: String) {
        self.name = name
    }
} """
        this.Test ("TypeCasting01", code)

    [<Test>]
    member this.TypeCasting02() =
        let code = """class Movie: MediaItem {
    var director: String
    init(name: String, director: String) {
        self.director = director
        super.init(name: name)
    }
}
 
class Song: MediaItem {
    var artist: String
    init(name: String, artist: String) {
        self.artist = artist
        super.init(name: name)
    }
} """
        this.Test ("TypeCasting02", code)

    [<Test>]
    member this.TypeCasting03() =
        let code = """let library = [
    Movie(name: "Casablanca", director: "Michael Curtiz"),
    Song(name: "Blue Suede Shoes", artist: "Elvis Presley"),
    Movie(name: "Citizen Kane", director: "Orson Welles"),
    Song(name: "The One And Only", artist: "Chesney Hawkes"),
    Song(name: "Never Gonna Give You Up", artist: "Rick Astley")
]
// the type of "library" is inferred to be [MediaItem] """
        this.Test ("TypeCasting03", code)

    [<Test>]
    member this.TypeCasting04() =
        let code = """var movieCount = 0
var songCount = 0
 
for item in library {
    if item is Movie {
        ++movieCount
    } else if item is Song {
        ++songCount
    }
}
 
println("Media library contains \(movieCount) movies and \(songCount) songs")
// prints "Media library contains 2 movies and 3 songs" """
        this.Test ("TypeCasting04", code)

    [<Test>]
    member this.TypeCasting05() =
        let code = """for item in library {
    if let movie = item as? Movie {
        println("Movie: '\(movie.name)', dir. \(movie.director)")
    } else if let song = item as? Song {
        println("Song: '\(song.name)', by \(song.artist)")
    }
}
 
// Movie: 'Casablanca', dir. Michael Curtiz
// Song: 'Blue Suede Shoes', by Elvis Presley
// Movie: 'Citizen Kane', dir. Orson Welles
// Song: 'The One And Only', by Chesney Hawkes
// Song: 'Never Gonna Give You Up', by Rick Astley """
        this.Test ("TypeCasting05", code)

    [<Test>]
    member this.TypeCasting06() =
        let code = """let someObjects: [AnyObject] = [
    Movie(name: "2001: A Space Odyssey", director: "Stanley Kubrick"),
    Movie(name: "Moon", director: "Duncan Jones"),
    Movie(name: "Alien", director: "Ridley Scott")
] """
        this.Test ("TypeCasting06", code)

    [<Test>]
    member this.TypeCasting07() =
        let code = """for object in someObjects {
    let movie = object as Movie
    println("Movie: '\(movie.name)', dir. \(movie.director)")
}
// Movie: '2001: A Space Odyssey', dir. Stanley Kubrick
// Movie: 'Moon', dir. Duncan Jones
// Movie: 'Alien', dir. Ridley Scott """
        this.Test ("TypeCasting07", code)

    [<Test>]
    member this.TypeCasting08() =
        let code = """for movie in someObjects as [Movie] {
    println("Movie: '\(movie.name)', dir. \(movie.director)")
}
// Movie: '2001: A Space Odyssey', dir. Stanley Kubrick
// Movie: 'Moon', dir. Duncan Jones
// Movie: 'Alien', dir. Ridley Scott """
        this.Test ("TypeCasting08", code)

    [<Test>]
    member this.TypeCasting09() =
        let code = """var things = [Any]()
 
things.append(0)
things.append(0.0)
things.append(42)
things.append(3.14159)
things.append("hello")
things.append((3.0, 5.0))
things.append(Movie(name: "Ghostbusters", director: "Ivan Reitman")) """
        this.Test ("TypeCasting09", code)

    [<Test>]
    member this.TypeCasting10() =
        let code = """for thing in things {
    switch thing {
    case 0 as Int:
        println("zero as an Int")
    case 0 as Double:
        println("zero as a Double")
    case let someInt as Int:
        println("an integer value of \(someInt)")
    case let someDouble as Double where someDouble > 0:
        println("a positive double value of \(someDouble)")
    case is Double:
        println("some other double value that I don't want to print")
    case let someString as String:
        println("a string value of \"\(someString)\"")
    case let (x, y) as (Double, Double):
        println("an (x, y) point at \(x), \(y)")
    case let movie as Movie:
        println("a movie called '\(movie.name)', dir. \(movie.director)")
    default:
        println("something else")
    }
}
 
// zero as an Int
// zero as a Double
// an integer value of 42
// a positive double value of 3.14159
// a string value of "hello"
// an (x, y) point at 3.0, 5.0
// a movie called 'Ghostbusters', dir. Ivan Reitman """
        this.Test ("TypeCasting10", code)

