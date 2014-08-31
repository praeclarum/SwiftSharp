namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type AutomaticReferenceCountingTests () =
    inherit BookTests ()

    [<Test>]
    member this.AutomaticReferenceCounting01() =
        let code = """class Person {
    let name: String
    init(name: String) {
        self.name = name
        println("\(name) is being initialized")
    }
    deinit {
        println("\(name) is being deinitialized")
    }
} """
        this.Test ("AutomaticReferenceCounting01", code)

    [<Test>]
    member this.AutomaticReferenceCounting02() =
        let code = """var reference1: Person?
var reference2: Person?
var reference3: Person? """
        this.Test ("AutomaticReferenceCounting02", code)

    [<Test>]
    member this.AutomaticReferenceCounting03() =
        let code = """reference1 = Person(name: "John Appleseed")
// prints "John Appleseed is being initialized" """
        this.Test ("AutomaticReferenceCounting03", code)

    [<Test>]
    member this.AutomaticReferenceCounting04() =
        let code = """reference2 = reference1
reference3 = reference1 """
        this.Test ("AutomaticReferenceCounting04", code)

    [<Test>]
    member this.AutomaticReferenceCounting05() =
        let code = """reference1 = nil
reference2 = nil """
        this.Test ("AutomaticReferenceCounting05", code)

    [<Test>]
    member this.AutomaticReferenceCounting06() =
        let code = """reference3 = nil
// prints "John Appleseed is being deinitialized" """
        this.Test ("AutomaticReferenceCounting06", code)

    [<Test>]
    member this.AutomaticReferenceCounting07() =
        let code = """class Person {
    let name: String
    init(name: String) { self.name = name }
    var apartment: Apartment?
    deinit { println("\(name) is being deinitialized") }
}
 
class Apartment {
    let number: Int
    init(number: Int) { self.number = number }
    var tenant: Person?
    deinit { println("Apartment #\(number) is being deinitialized") }
} """
        this.Test ("AutomaticReferenceCounting07", code)

    [<Test>]
    member this.AutomaticReferenceCounting08() =
        let code = """var john: Person?
var number73: Apartment? """
        this.Test ("AutomaticReferenceCounting08", code)

    [<Test>]
    member this.AutomaticReferenceCounting09() =
        let code = """john = Person(name: "John Appleseed")
number73 = Apartment(number: 73) """
        this.Test ("AutomaticReferenceCounting09", code)

    [<Test>]
    member this.AutomaticReferenceCounting10() =
        let code = """john!.apartment = number73
number73!.tenant = john """
        this.Test ("AutomaticReferenceCounting10", code)

    [<Test>]
    member this.AutomaticReferenceCounting11() =
        let code = """john = nil
number73 = nil """
        this.Test ("AutomaticReferenceCounting11", code)

    [<Test>]
    member this.AutomaticReferenceCounting12() =
        let code = """class Person {
    let name: String
    init(name: String) { self.name = name }
    var apartment: Apartment?
    deinit { println("\(name) is being deinitialized") }
}
 
class Apartment {
    let number: Int
    init(number: Int) { self.number = number }
    weak var tenant: Person?
    deinit { println("Apartment #\(number) is being deinitialized") }
} """
        this.Test ("AutomaticReferenceCounting12", code)

    [<Test>]
    member this.AutomaticReferenceCounting13() =
        let code = """var john: Person?
var number73: Apartment?
 
john = Person(name: "John Appleseed")
number73 = Apartment(number: 73)
 
john!.apartment = number73
number73!.tenant = john """
        this.Test ("AutomaticReferenceCounting13", code)

    [<Test>]
    member this.AutomaticReferenceCounting14() =
        let code = """john = nil
// prints "John Appleseed is being deinitialized" """
        this.Test ("AutomaticReferenceCounting14", code)

    [<Test>]
    member this.AutomaticReferenceCounting15() =
        let code = """number73 = nil
// prints "Apartment #73 is being deinitialized" """
        this.Test ("AutomaticReferenceCounting15", code)

    [<Test>]
    member this.AutomaticReferenceCounting16() =
        let code = """class Customer {
    let name: String
    var card: CreditCard?
    init(name: String) {
        self.name = name
    }
    deinit { println("\(name) is being deinitialized") }
}
 
class CreditCard {
    let number: UInt64
    unowned let customer: Customer
    init(number: UInt64, customer: Customer) {
        self.number = number
        self.customer = customer
    }
    deinit { println("Card #\(number) is being deinitialized") }
} """
        this.Test ("AutomaticReferenceCounting16", code)

    [<Test>]
    member this.AutomaticReferenceCounting17() =
        let code = """var john: Customer? """
        this.Test ("AutomaticReferenceCounting17", code)

    [<Test>]
    member this.AutomaticReferenceCounting18() =
        let code = """john = Customer(name: "John Appleseed")
john!.card = CreditCard(number: 1234_5678_9012_3456, customer: john!) """
        this.Test ("AutomaticReferenceCounting18", code)

    [<Test>]
    member this.AutomaticReferenceCounting19() =
        let code = """john = nil
// prints "John Appleseed is being deinitialized"
// prints "Card #1234567890123456 is being deinitialized" """
        this.Test ("AutomaticReferenceCounting19", code)

    [<Test>]
    member this.AutomaticReferenceCounting20() =
        let code = """class Country {
    let name: String
    let capitalCity: City!
    init(name: String, capitalName: String) {
        self.name = name
        self.capitalCity = City(name: capitalName, country: self)
    }
}
 
class City {
    let name: String
    unowned let country: Country
    init(name: String, country: Country) {
        self.name = name
        self.country = country
    }
} """
        this.Test ("AutomaticReferenceCounting20", code)

    [<Test>]
    member this.AutomaticReferenceCounting21() =
        let code = """var country = Country(name: "Canada", capitalName: "Ottawa")
println("\(country.name)'s capital city is called \(country.capitalCity.name)")
// prints "Canada's capital city is called Ottawa" """
        this.Test ("AutomaticReferenceCounting21", code)

    [<Test>]
    member this.AutomaticReferenceCounting22() =
        let code = """class HTMLElement {
    
    let name: String
    let text: String?
    
    lazy var asHTML: () -> String = {
        if let text = self.text {
            return "<\(self.name)>\(text)</\(self.name)>"
        } else {
            return "<\(self.name) />"
        }
    }
    
    init(name: String, text: String? = nil) {
        self.name = name
        self.text = text
    }
    
    deinit {
        println("\(name) is being deinitialized")
    }
    
} """
        this.Test ("AutomaticReferenceCounting22", code)

    [<Test>]
    member this.AutomaticReferenceCounting23() =
        let code = """var paragraph: HTMLElement? = HTMLElement(name: "p", text: "hello, world")
println(paragraph!.asHTML())
// prints "<p>hello, world</p>" """
        this.Test ("AutomaticReferenceCounting23", code)

    [<Test>]
    member this.AutomaticReferenceCounting24() =
        let code = """paragraph = nil """
        this.Test ("AutomaticReferenceCounting24", code)

    [<Test>]
    member this.AutomaticReferenceCounting25() =
        let code = """lazy var someClosure: (Int, String) -> String = {
    [unowned self] (index: Int, stringToProcess: String) -> String in
    // closure body goes here
} """
        this.Test ("AutomaticReferenceCounting25", code)

    [<Test>]
    member this.AutomaticReferenceCounting26() =
        let code = """lazy var someClosure: () -> String = {
    [unowned self] in
    // closure body goes here
} """
        this.Test ("AutomaticReferenceCounting26", code)

    [<Test>]
    member this.AutomaticReferenceCounting27() =
        let code = """class HTMLElement {
    
    let name: String
    let text: String?
    
    lazy var asHTML: () -> String = {
        [unowned self] in
        if let text = self.text {
            return "<\(self.name)>\(text)</\(self.name)>"
        } else {
            return "<\(self.name) />"
        }
    }
    
    init(name: String, text: String? = nil) {
        self.name = name
        self.text = text
    }
    
    deinit {
        println("\(name) is being deinitialized")
    }
    
} """
        this.Test ("AutomaticReferenceCounting27", code)

    [<Test>]
    member this.AutomaticReferenceCounting28() =
        let code = """var paragraph: HTMLElement? = HTMLElement(name: "p", text: "hello, world")
println(paragraph!.asHTML())
// prints "<p>hello, world</p>" """
        this.Test ("AutomaticReferenceCounting28", code)

    [<Test>]
    member this.AutomaticReferenceCounting29() =
        let code = """paragraph = nil
// prints "p is being deinitialized" """
        this.Test ("AutomaticReferenceCounting29", code)

