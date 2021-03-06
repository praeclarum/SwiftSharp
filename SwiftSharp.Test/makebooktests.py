#!/usr/bin/env python

import glob
import re
import sys
from xml.dom import minidom

def write(f, line):
	f.write(line.encode("UTF-8"))


def writeText (f, c):
	if c.nodeType == c.TEXT_NODE: write(f, c.nodeValue)
	elif c.nodeType == c.ELEMENT_NODE:
		for sc in c.childNodes: writeText(f, sc)

def writeCode (f, ul):
	head = u""
	for li in [x for x in ul.childNodes if x.nodeType == ul.ELEMENT_NODE and x.tagName == "li"]:
		write(f, head)
		for code in [x for x in li.childNodes if x.nodeType == ul.ELEMENT_NODE and x.tagName == "code"]:
			writeText (f, code)
		head = u"\n"
		

def writeTest (f, name, id, codeLines):
	write(f, u"    [<Test>]\n")
	write(f, u"    member this.%s%.2d() =\n" % (name, id))
	write(f, u"        let code = \"\"\"")
	writeCode (f, codeLines)
	write(f, u" \"\"\"\n")
	write(f, u"        this.Test (\"%s%.2d\", code)\n\n" % (name, id))

def writeTestFixture(name):
	print name
	f = open(name + "Tests.fs", "wb")

	write(f, u"namespace SwiftSharp.Test\n")
	write(f, u"open NUnit.Framework\n\n")

	write(f, u"[<TestFixture>]\n")
	write(f, u"type %sTests () =\n" % name)
	write(f, u"    inherit BookTests ()\n\n")
	doc = minidom.parse("TestFiles/" + name + ".html")
	divs = doc.getElementsByTagName ("ul")
	id = 1
	for e in divs:
		if e.hasAttribute ("class") and e.attributes["class"].value == "code-lines":
			writeTest (f, name, id, e)
			id = id + 1
	f.close ()

for p in glob.glob("TestFiles/*.html"):
	writeTestFixture (re.search(r"/(\w+)\.html", p).groups(1)[0])
