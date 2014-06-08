
all: Swiften/SwiftParser.Generated.cs


test: Tests/HelloWorld.swift ./Swiften.Compiler/bin/Debug/Swiften.Compiler.exe
	mono ./Swiften.Compiler/bin/Debug/Swiften.Compiler.exe Tests/HelloWorld.swift
	mono ./Tests/HelloWorld.exe

Swiften/SwiftParser.Generated.cs: Swiften/SwiftParser.jay ./Lib/skeleton.cs
	./Lib/jay -c -v Swiften/SwiftParser.jay < ./Lib/skeleton.cs > Swiften/SwiftParser.Generated.cs


