namespace SwiftSharp.Test
open NUnit.Framework

[<TestFixture>]
type AutomaticReferenceCountingTests () =
    inherit BookTests ()

    [<Test>]
    member this.Sample1() =
        let code = """
class Person {
    let name: String
    init(name: String) {
        self.name = name
        println("\(name) is being initialized")
    }
    deinit {
        println("\(name) is being deinitialized")
    }
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample2() =
        let code = """
var reference1: Person?
var reference2: Person?
var reference3: Person?
        """
        this.Test (code)

    [<Test>]
    member this.Sample3() =
        let code = """
reference1 = Person(name: "John Appleseed")
// prints "John Appleseed is being initialized"
        """
        this.Test (code)

    [<Test>]
    member this.Sample4() =
        let code = """
reference2 = reference1
reference3 = reference1
        """
        this.Test (code)

    [<Test>]
    member this.Sample5() =
        let code = """
reference1 = nil
reference2 = nil
        """
        this.Test (code)

    [<Test>]
    member this.Sample6() =
        let code = """
reference3 = nil
// prints "John Appleseed is being deinitialized"
        """
        this.Test (code)

    [<Test>]
    member this.Sample7() =
        let code = """
class Person {
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
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample8() =
        let code = """
var john: Person?
var number73: Apartment?
        """
        this.Test (code)

    [<Test>]
    member this.Sample9() =
        let code = """
john = Person(name: "John Appleseed")
number73 = Apartment(number: 73)
        """
        this.Test (code)

    [<Test>]
    member this.Sample10() =
        let code = """
john!.apartment = number73
number73!.tenant = john
        """
        this.Test (code)

    [<Test>]
    member this.Sample11() =
        let code = """
john = nil
number73 = nil
        """
        this.Test (code)

    [<Test>]
    member this.Sample12() =
        let code = """
class Person {
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
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample13() =
        let code = """
var john: Person?
var number73: Apartment?
 
john = Person(name: "John Appleseed")
number73 = Apartment(number: 73)
 
john!.apartment = number73
number73!.tenant = john
        """
        this.Test (code)

    [<Test>]
    member this.Sample14() =
        let code = """
john = nil
// prints "John Appleseed is being deinitialized"
        """
        this.Test (code)

    [<Test>]
    member this.Sample15() =
        let code = """
number73 = nil
// prints "Apartment #73 is being deinitialized"
        """
        this.Test (code)

    [<Test>]
    member this.Sample16() =
        let code = """
class Customer {
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
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample17() =
        let code = """
var john: Customer?
        """
        this.Test (code)

    [<Test>]
    member this.Sample18() =
        let code = """
john = Customer(name: "John Appleseed")
john!.card = CreditCard(number: 1234_5678_9012_3456, customer: john!)
        """
        this.Test (code)

    [<Test>]
    member this.Sample19() =
        let code = """
john = nil
// prints "John Appleseed is being deinitialized"
// prints "Card #1234567890123456 is being deinitialized"
        """
        this.Test (code)

    [<Test>]
    member this.Sample20() =
        let code = """
class Country {
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
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample21() =
        let code = """
var country = Country(name: "Canada", capitalName: "Ottawa")
println("\(country.name)'s capital city is called \(country.capitalCity.name)")
// prints "Canada's capital city is called Ottawa"
        """
        this.Test (code)

    [<Test>]
    member this.Sample22() =
        let code = """
class HTMLElement {
    
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
    
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample23() =
        let code = """
var paragraph: HTMLElement? = HTMLElement(name: "p", text: "hello, world")
println(paragraph!.asHTML())
// prints "<p>hello, world</p>"
        """
        this.Test (code)

    [<Test>]
    member this.Sample24() =
        let code = """
paragraph = nil
        """
        this.Test (code)

    [<Test>]
    member this.Sample25() =
        let code = """
lazy var someClosure: (Int, String) -> String = {
    [unowned self] (index: Int, stringToProcess: String) -> String in
    // closure body goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample26() =
        let code = """
lazy var someClosure: () -> String = {
    [unowned self] in
    // closure body goes here
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample27() =
        let code = """
class HTMLElement {
    
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
    
}
        """
        this.Test (code)

    [<Test>]
    member this.Sample28() =
        let code = """
var paragraph: HTMLElement? = HTMLElement(name: "p", text: "hello, world")
println(paragraph!.asHTML())
// prints "<p>hello, world</p>"
        """
        this.Test (code)

    [<Test>]
    member this.Sample29() =
        let code = """
paragraph = nil
// prints "p is being deinitialized"
        """
        this.Test (code)

