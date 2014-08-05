import CoreFoundation
import CoreFoundation
import Dispatch
import Foundation

/*	NSString.h
	Copyright (c) 1994-2014, Apple Inc. All rights reserved.
*/

typealias unichar = UInt16

let NSParseErrorException: NSString! // raised by -propertyList

/* These options apply to the various search/find and comparison methods (except where noted).
*/
struct NSStringCompareOptions : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var CaseInsensitiveSearch: NSStringCompareOptions { get }
    static var LiteralSearch: NSStringCompareOptions { get } /* Exact character-by-character equivalence */
    static var BackwardsSearch: NSStringCompareOptions { get } /* Search from end of source string */
    static var AnchoredSearch: NSStringCompareOptions { get } /* Search is limited to start (or end, if NSBackwardsSearch) of source string */
    static var NumericSearch: NSStringCompareOptions { get } /* Added in 10.2; Numbers within strings are compared using numeric value, that is, Foo2.txt < Foo7.txt < Foo25.txt; only applies to compare methods, not find */
    static var DiacriticInsensitiveSearch: NSStringCompareOptions { get } /* If specified, ignores diacritics (o-umlaut == o) */
    static var WidthInsensitiveSearch: NSStringCompareOptions { get } /* If specified, ignores width differences ('a' == UFF41) */
    static var ForcedOrderingSearch: NSStringCompareOptions { get } /* If specified, comparisons are forced to return either NSOrderedAscending or NSOrderedDescending if the strings are equivalent but not strictly equal, for stability when sorting (e.g. "aaa" > "AAA" with NSCaseInsensitiveSearch specified) */
    static var RegularExpressionSearch: NSStringCompareOptions { get } /* Applies to rangeOfString:..., stringByReplacingOccurrencesOfString:..., and replaceOccurrencesOfString:... methods only; the search string is treated as an ICU-compatible regular expression; if set, no other options can apply except NSCaseInsensitiveSearch and NSAnchoredSearch */
}

/* Note that in addition to the values explicitly listed below, NSStringEncoding supports encodings provided by CFString.
See CFStringEncodingExt.h for a list of these encodings.
See CFString.h for functions which convert between NSStringEncoding and CFStringEncoding.
*/

var NSASCIIStringEncoding: UInt32 { get } /* 0..127 only */
var NSNEXTSTEPStringEncoding: UInt32 { get }
var NSJapaneseEUCStringEncoding: UInt32 { get }
var NSUTF8StringEncoding: UInt32 { get }
var NSISOLatin1StringEncoding: UInt32 { get }
var NSSymbolStringEncoding: UInt32 { get }
var NSNonLossyASCIIStringEncoding: UInt32 { get }
var NSShiftJISStringEncoding: UInt32 { get } /* kCFStringEncodingDOSJapanese */
var NSISOLatin2StringEncoding: UInt32 { get }
var NSUnicodeStringEncoding: UInt32 { get }
var NSWindowsCP1251StringEncoding: UInt32 { get } /* Cyrillic; same as AdobeStandardCyrillic */
var NSWindowsCP1252StringEncoding: UInt32 { get } /* WinLatin1 */
var NSWindowsCP1253StringEncoding: UInt32 { get } /* Greek */
var NSWindowsCP1254StringEncoding: UInt32 { get } /* Turkish */
var NSWindowsCP1250StringEncoding: UInt32 { get } /* WinLatin2 */
var NSISO2022JPStringEncoding: UInt32 { get } /* ISO 2022 Japanese encoding for e-mail */
var NSMacOSRomanStringEncoding: UInt32 { get }

var NSUTF16StringEncoding: UInt32 { get } /* An alias for NSUnicodeStringEncoding */

var NSUTF16BigEndianStringEncoding: UInt32 { get } /* NSUTF16StringEncoding encoding with explicit endianness specified */
var NSUTF16LittleEndianStringEncoding: UInt32 { get } /* NSUTF16StringEncoding encoding with explicit endianness specified */

var NSUTF32StringEncoding: UInt32 { get }
var NSUTF32BigEndianStringEncoding: UInt32 { get } /* NSUTF32StringEncoding encoding with explicit endianness specified */
var NSUTF32LittleEndianStringEncoding: UInt32 { get } /* NSUTF32StringEncoding encoding with explicit endianness specified */

struct NSStringEncodingConversionOptions : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var AllowLossy: NSStringEncodingConversionOptions { get }
    static var ExternalRepresentation: NSStringEncodingConversionOptions { get }
}

let NSCharacterConversionException: NSString!

class NSString : NSObject, NSCopying, NSMutableCopying, NSSecureCoding, NSCoding {
    
    /* NSString primitive (funnel) methods. A minimal subclass of NSString just needs to implement these, although we also recommend getCharacters:range:. See below for the other methods.
    */
    var length: Int { get }
    func characterAtIndex(index: Int) -> unichar
}

extension NSString : StringLiteralConvertible {
    class func convertFromExtendedGraphemeClusterLiteral(value: StaticString) -> Self
    class func convertFromStringLiteral(value: StaticString) -> Self
}



extension NSString {
}

extension NSString : Reflectable {
    func getMirror() -> Mirror
}

extension NSString {
    convenience init(format: NSString, _ args: CVarArg...)
    convenience init(format: NSString, locale: NSLocale?, _ args: CVarArg...)
    class func localizedStringWithFormat(format: NSString, _ args: CVarArg...) -> NSString
    func stringByAppendingFormat(format: NSString, _ args: CVarArg...) -> NSString
}

extension NSString {
    
    func getCharacters(buffer: UnsafePointer<unichar>, range aRange: NSRange)
    
    func substringFromIndex(from: Int) -> String!
    func substringToIndex(to: Int) -> String!
    func substringWithRange(range: NSRange) -> String! // Hint: Use with rangeOfComposedCharacterSequencesForRange: to avoid breaking up composed characters
    
    /* In the compare: methods, the range argument specifies the subrange, rather than the whole, of the receiver to use in the comparison. The range is not applied to the search string.  For example, [@"AB" compare:@"ABC" options:0 range:NSMakeRange(0,1)] compares "A" to "ABC", not "A" to "A", and will return NSOrderedAscending.
    */
    func compare(string: String!) -> NSComparisonResult
    func compare(string: String!, options mask: NSStringCompareOptions) -> NSComparisonResult
    func compare(string: String!, options mask: NSStringCompareOptions, range compareRange: NSRange) -> NSComparisonResult
    func compare(string: String!, options mask: NSStringCompareOptions, range compareRange: NSRange, locale: AnyObject!) -> NSComparisonResult // locale arg used to be a dictionary pre-Leopard. We now accept NSLocale. Assumes the current locale if non-nil and non-NSLocale. nil continues to mean canonical compare, which doesn't depend on user's locale choice.
    func caseInsensitiveCompare(string: String!) -> NSComparisonResult
    func localizedCompare(string: String!) -> NSComparisonResult
    func localizedCaseInsensitiveCompare(string: String!) -> NSComparisonResult
    
    /* localizedStandardCompare:, added in 10.6, should be used whenever file names or other strings are presented in lists and tables where Finder-like sorting is appropriate.  The exact behavior of this method may be tweaked in future releases, and will be different under different localizations, so clients should not depend on the exact sorting order of the strings.
    */
    func localizedStandardCompare(string: String!) -> NSComparisonResult
    
    func isEqualToString(aString: String!) -> Bool
    
    func hasPrefix(aString: String!) -> Bool
    func hasSuffix(aString: String!) -> Bool
    
    /* containsString: returns YES if the target string is contained within the receiver. Same as calling rangeOfString:options: with no options, thus doing a case-sensitive, non-literal search. localizedCaseInsensitiveContainsString: is the case-insensitive variant. Note that it takes the current locale into effect as well.  Locale-independent case-insensitive operation, and other needs can be achieved by calling rangeOfString:options:range:locale: directly.
     */
    func containsString(aString: String!) -> Bool
    func localizedCaseInsensitiveContainsString(aString: String!) -> Bool
    
    /* These methods return length==0 if the target string is not found. So, to check for containment: ([str rangeOfString:@"target"].length > 0).  Note that the length of the range returned by these methods might be different than the length of the target string, due composed characters and such.
    */
    func rangeOfString(aString: String!) -> NSRange
    func rangeOfString(aString: String!, options mask: NSStringCompareOptions) -> NSRange
    func rangeOfString(aString: String!, options mask: NSStringCompareOptions, range searchRange: NSRange) -> NSRange
    func rangeOfString(aString: String!, options mask: NSStringCompareOptions, range searchRange: NSRange, locale: NSLocale!) -> NSRange
    
    /* These return the range of the first character from the set in the string, not the range of a sequence of characters. 
    */
    func rangeOfCharacterFromSet(aSet: NSCharacterSet!) -> NSRange
    func rangeOfCharacterFromSet(aSet: NSCharacterSet!, options mask: NSStringCompareOptions) -> NSRange
    func rangeOfCharacterFromSet(aSet: NSCharacterSet!, options mask: NSStringCompareOptions, range searchRange: NSRange) -> NSRange
    
    func rangeOfComposedCharacterSequenceAtIndex(index: Int) -> NSRange
    func rangeOfComposedCharacterSequencesForRange(range: NSRange) -> NSRange
    
    func stringByAppendingString(aString: String!) -> String!
    
    /* The following convenience methods all skip initial space characters (whitespaceSet) and ignore trailing characters. NSScanner can be used for more "exact" parsing of numbers.
    */
    var doubleValue: Double { get }
    var floatValue: Float { get }
    var intValue: Int32 { get }
    var integerValue: Int { get }
    var longLongValue: Int64 { get }
    var boolValue: Bool { get } // Skips initial space characters (whitespaceSet), or optional -/+ sign followed by zeroes. Returns YES on encountering one of "Y", "y", "T", "t", or a digit 1-9. It ignores any trailing characters.
    
    func componentsSeparatedByString(separator: String!) -> [AnyObject]!
    func componentsSeparatedByCharactersInSet(separator: NSCharacterSet!) -> [AnyObject]!
    
    func commonPrefixWithString(aString: String!, options mask: NSStringCompareOptions) -> String!
    
    /* The following three case methods perform the canonical (non-localized) mappings. They are suitable for programming operations that require stable results not depending on the user's locale preference.  For localized case mapping for strings presented to users, use their corresponding methods with locale argument below.
     */
    var uppercaseString: String! { get }
    var lowercaseString: String! { get }
    var capitalizedString: String! { get }
    
    /* The following methods perform localized case mappings based on the locale specified. Passing nil indicates the canonical mapping.  For the user preference locale setting, specify +[NSLocale currentLocale].
     */
    func uppercaseStringWithLocale(locale: NSLocale!) -> String!
    func lowercaseStringWithLocale(locale: NSLocale!) -> String!
    func capitalizedStringWithLocale(locale: NSLocale!) -> String!
    
    func stringByTrimmingCharactersInSet(set: NSCharacterSet!) -> String!
    func stringByPaddingToLength(newLength: Int, withString padString: String!, startingAtIndex padIndex: Int) -> String!
    
    func getLineStart(startPtr: UnsafePointer<Int>, end lineEndPtr: UnsafePointer<Int>, contentsEnd contentsEndPtr: UnsafePointer<Int>, forRange range: NSRange)
    func lineRangeForRange(range: NSRange) -> NSRange
    
    func getParagraphStart(startPtr: UnsafePointer<Int>, end parEndPtr: UnsafePointer<Int>, contentsEnd contentsEndPtr: UnsafePointer<Int>, forRange range: NSRange)
    func paragraphRangeForRange(range: NSRange) -> NSRange
    
    // Pass in one of the "By" options:
    // Equivalent to lineRangeForRange:
    // Equivalent to paragraphRangeForRange:
    // Equivalent to rangeOfComposedCharacterSequencesForRange:
    
    // ...and combine any of the desired additional options:
    
    // User's default locale
    
    /* In the enumerate methods, the blocks will be invoked inside an autorelease pool, so any values assigned inside the block should be retained.
    */
    func enumerateSubstringsInRange(range: NSRange, options opts: NSStringEnumerationOptions, usingBlock block: ((String!, NSRange, NSRange, UnsafePointer<ObjCBool>) -> Void)!)
    func enumerateLinesUsingBlock(block: ((String!, UnsafePointer<ObjCBool>) -> Void)!)
    
    var description: String! { get }
    
    var hash: Int { get }
    
    
    /*** Encoding methods ***/
    var fastestEncoding: UInt { get } // Result in O(1) time; a rough estimate
    var smallestEncoding: UInt { get } // Result in O(n) time; the encoding in which the string is most compact
    
    func dataUsingEncoding(encoding: UInt, allowLossyConversion lossy: Bool) -> NSData! // External representation
    func dataUsingEncoding(encoding: UInt) -> NSData! // External representation
    
    func canBeConvertedToEncoding(encoding: UInt) -> Bool
    
    /* Methods to convert NSString to a NULL-terminated cString using the specified encoding. Note, these are the "new" cString methods, and are not deprecated like the older cString methods which do not take encoding arguments.
    */
    func cStringUsingEncoding(encoding: UInt) -> CString // "Autoreleased"; NULL return if encoding conversion not possible; for performance reasons, lifetime of this should not be considered longer than the lifetime of the receiving string (if the receiver string is freed, this might go invalid then, before the end of the autorelease scope)
    func getCString(buffer: UnsafePointer<Int8>, maxLength maxBufferCount: Int, encoding: UInt) -> Bool // NO return if conversion not possible due to encoding errors or too small of a buffer. The buffer should include room for maxBufferCount bytes; this number should accomodate the expected size of the return value plus the NULL termination character, which this method adds. (So note that the maxLength passed to this method is one more than the one you would have passed to the deprecated getCString:maxLength:.)
    
    /* Use this to convert string section at a time into a fixed-size buffer, without any allocations.  Does not NULL-terminate. 
        buffer is the buffer to write to; if NULL, this method can be used to computed size of needed buffer.
        maxBufferCount is the length of the buffer in bytes. It's a good idea to make sure this is at least enough to hold one character's worth of conversion. 
        usedBufferCount is the length of the buffer used up by the current conversion. Can be NULL.
        encoding is the encoding to convert to.
        options specifies the options to apply.
        range is the range to convert.
        leftOver is the remaining range. Can be NULL.
        YES return indicates some characters were converted. Conversion might usually stop when the buffer fills, 
          but it might also stop when the conversion isn't possible due to the chosen encoding. 
    */
    func getBytes(buffer: UnsafePointer<()>, maxLength maxBufferCount: Int, usedLength usedBufferCount: UnsafePointer<Int>, encoding: UInt, options: NSStringEncodingConversionOptions, range: NSRange, remainingRange leftover: NSRangePointer) -> Bool
    
    /* These return the maximum and exact number of bytes needed to store the receiver in the specified encoding in non-external representation. The first one is O(1), while the second one is O(n). These do not include space for a terminating null.
    */
    func maximumLengthOfBytesUsingEncoding(enc: UInt) -> Int // Result in O(1) time; the estimate may be way over what's needed. Returns 0 on error (overflow)
    func lengthOfBytesUsingEncoding(enc: UInt) -> Int // Result in O(n) time; the result is exact. Returns 0 on error (cannot convert to specified encoding, or overflow)
    
    var decomposedStringWithCanonicalMapping: String! { get }
    var precomposedStringWithCanonicalMapping: String! { get }
    var decomposedStringWithCompatibilityMapping: String! { get }
    var precomposedStringWithCompatibilityMapping: String! { get }
    
    /* Returns a string with the character folding options applied. theOptions is a mask of compare flags with *InsensitiveSearch suffix.
    */
    func stringByFoldingWithOptions(options: NSStringCompareOptions, locale: NSLocale!) -> String!
    
    /* Replace all occurrences of the target string in the specified range with replacement. Specified compare options are used for matching target. If NSRegularExpressionSearch is specified, the replacement is treated as a template, as in the corresponding NSRegularExpression methods, and no other options can apply except NSCaseInsensitiveSearch and NSAnchoredSearch.
    */
    func stringByReplacingOccurrencesOfString(target: String!, withString replacement: String!, options: NSStringCompareOptions, range searchRange: NSRange) -> String!
    
    /* Replace all occurrences of the target string with replacement. Invokes the above method with 0 options and range of the whole string.
    */
    func stringByReplacingOccurrencesOfString(target: String!, withString replacement: String!) -> String!
    
    /* Replace characters in range with the specified string, returning new string.
    */
    func stringByReplacingCharactersInRange(range: NSRange, withString replacement: String!) -> String!
    
    var UTF8String: CString { get } // Convenience to return null-terminated UTF8 representation
    
    /* User-dependent encoding who value is derived from user's default language and potentially other factors. The use of this encoding might sometimes be needed when interpreting user documents with unknown encodings, in the absence of other hints.  This encoding should be used rarely, if at all. Note that some potential values here might result in unexpected encoding conversions of even fairly straightforward NSString content --- for instance, punctuation characters with a bidirectional encoding.
    */
    class func defaultCStringEncoding() -> UInt // Should be rarely used
    
    class func availableStringEncodings() -> ConstUnsafePointer<UInt>
    class func localizedNameOfStringEncoding(encoding: UInt) -> String!
    
    
    /* In general creation methods in NSString do not apply to subclassers, as subclassers are assumed to provide their own init methods which create the string in the way the subclass wishes.  Designated initializers of NSString are thus init and initWithCoder:.
    */
    /*** Creation methods ***/
    init()
    init(charactersNoCopy characters: UnsafePointer<unichar>, length: Int, freeWhenDone freeBuffer: Bool) /* "NoCopy" is a hint */
    init(characters: ConstUnsafePointer<unichar>, length: Int)
    init(UTF8String nullTerminatedCString: CString)
    init(string aString: String!)
    
    init(format: String!, arguments argList: CVaListPointer)
    
    init(format: String!, locale: AnyObject!, arguments argList: CVaListPointer)
    init(data: NSData!, encoding: UInt)
    init(bytes: ConstUnsafePointer<()>, length len: Int, encoding: UInt)
    init(bytesNoCopy bytes: UnsafePointer<()>, length len: Int, encoding: UInt, freeWhenDone freeBuffer: Bool) /* "NoCopy" is a hint */
    
    class func string() -> Self!
    class func stringWithString(string: String!) -> Self!
    class func stringWithCharacters(characters: ConstUnsafePointer<unichar>, length: Int) -> Self!
    class func stringWithUTF8String(nullTerminatedCString: CString) -> Self!
    
    init(CString nullTerminatedCString: CString, encoding: UInt)
    class func stringWithCString(cString: CString, encoding enc: UInt) -> Self!
    
    /* These use the specified encoding.  If nil is returned, the optional error return indicates problem that was encountered (for instance, file system or encoding errors).
    */
    init(contentsOfURL url: NSURL!, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>)
    init(contentsOfFile path: String!, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>)
    class func stringWithContentsOfURL(url: NSURL!, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>) -> Self!
    class func stringWithContentsOfFile(path: String!, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>) -> Self!
    
    /* These try to determine the encoding, and return the encoding which was used.  Note that these methods might get "smarter" in subsequent releases of the system, and use additional techniques for recognizing encodings. If nil is returned, the optional error return indicates problem that was encountered (for instance, file system or encoding errors).
    */
    init(contentsOfURL url: NSURL!, usedEncoding enc: UnsafePointer<UInt>, error: AutoreleasingUnsafePointer<NSError?>)
    init(contentsOfFile path: String!, usedEncoding enc: UnsafePointer<UInt>, error: AutoreleasingUnsafePointer<NSError?>)
    class func stringWithContentsOfURL(url: NSURL!, usedEncoding enc: UnsafePointer<UInt>, error: AutoreleasingUnsafePointer<NSError?>) -> Self!
    class func stringWithContentsOfFile(path: String!, usedEncoding enc: UnsafePointer<UInt>, error: AutoreleasingUnsafePointer<NSError?>) -> Self!
    
    /* Write to specified url or path using the specified encoding.  The optional error return is to indicate file system or encoding errors.
    */
    func writeToURL(url: NSURL!, atomically useAuxiliaryFile: Bool, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>) -> Bool
    func writeToFile(path: String!, atomically useAuxiliaryFile: Bool, encoding enc: UInt, error: AutoreleasingUnsafePointer<NSError?>) -> Bool
}
struct NSStringEnumerationOptions : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var ByLines: NSStringEnumerationOptions { get }
    static var ByParagraphs: NSStringEnumerationOptions { get }
    static var ByComposedCharacterSequences: NSStringEnumerationOptions { get }
    static var ByWords: NSStringEnumerationOptions { get }
    static var BySentences: NSStringEnumerationOptions { get }
    static var Reverse: NSStringEnumerationOptions { get }
    static var SubstringNotRequired: NSStringEnumerationOptions { get }
    static var Localized: NSStringEnumerationOptions { get }
}

class NSMutableString : NSString {
    
    /* NSMutableString primitive (funnel) method. See below for the other mutation methods.
    */
    func replaceCharactersInRange(range: NSRange, withString aString: String!)
}

extension NSMutableString {
    func appendFormat(format: NSString, _ args: CVarArg...)
}

extension NSMutableString {
    
    func insertString(aString: String!, atIndex loc: Int)
    func deleteCharactersInRange(range: NSRange)
    func appendString(aString: String!)
    
    func setString(aString: String!)
    
    /* In addition to these two, NSMutableString responds properly to all NSString creation methods.
    */
    init(capacity: Int)
    class func stringWithCapacity(capacity: Int) -> NSMutableString!
    
    /* This method replaces all occurrences of the target string with the replacement string, in the specified range of the receiver string, and returns the number of replacements. NSBackwardsSearch means the search is done from the end of the range (the results could be different); NSAnchoredSearch means only anchored (but potentially multiple) instances will be replaced. NSLiteralSearch and NSCaseInsensitiveSearch also apply. NSNumericSearch is ignored. Use NSMakeRange(0, [receiver length]) to process whole string. If NSRegularExpressionSearch is specified, the replacement is treated as a template, as in the corresponding NSRegularExpression methods, and no other options can apply except NSCaseInsensitiveSearch and NSAnchoredSearch.
    */
    func replaceOccurrencesOfString(target: String!, withString replacement: String!, options: NSStringCompareOptions, range searchRange: NSRange) -> Int
}

extension NSString {
    
    /*
     This API is used to detect the string encoding of a given raw data. It can also do lossy string conversion. It converts the data to a string in the detected string encoding. The data object contains the raw bytes, and the option dictionary contains the hints and parameters for the analysis. The opts dictionary can be nil. If the string parameter is not NULL, the string created by the detected string encoding is returned. The lossy substitution string is emitted in the output string for characters that could not be converted when lossy conversion is enabled. The usedLossyConversion indicates if there is any lossy conversion in the resulted string. If no encoding can be detected, 0 is returned.
     
     The possible items for the dictionary are:
     1) an array of suggested string encodings (without specifying the 3rd option in this list, all string encodings are considered but the ones in the array will have a higher preference; moreover, the order of the encodings in the array is important: the first encoding has a higher preference than the second one in the array)
     2) an array of string encodings not to use (the string encodings in this list will not be considered at all)
     3) a boolean option indicating whether only the suggested string encodings are considered
     4) a boolean option indicating whether lossy is allowed
     5) an option that gives a specific string to substitude for mystery bytes
     6) the current user's language
     7) a boolean option indicating whether the data is generated by Windows
     
     If the values in the dictionary have wrong types (for example, the value of NSStringEncodingDetectionSuggestedEncodingsKey is not an array), an exception is thrown.
     If the values in the dictionary are unknown (for example, the value in the array of suggested string encodings is not a valid encoding), the values will be ignored.
     */
    class func stringEncodingForData(data: NSData!, encodingOptions opts: [NSObject : AnyObject]!, convertedString string: AutoreleasingUnsafePointer<NSString?>, usedLossyConversion: UnsafePointer<ObjCBool>) -> UInt
}

/*
 The following keys may be used in the option dictionary.
 */
// NSArray of NSNumbers which contain NSStringEncoding values; if this key is not present in the dictionary, all encodings are weighted the same
let NSStringEncodingDetectionSuggestedEncodingsKey: NSString!

// NSArray of NSNumbers which contain NSStringEncoding values; if this key is not present in the dictionary, all encodings are considered
let NSStringEncodingDetectionDisallowedEncodingsKey: NSString!

// NSNumber boolean value; if this key is not present in the dictionary, the default value is NO
let NSStringEncodingDetectionUseOnlySuggestedEncodingsKey: NSString!

// NSNumber boolean value; if this key is not present in the dictionary, the default value is YES
let NSStringEncodingDetectionAllowLossyKey: NSString!

// NSNumber boolean value; if this key is not present in the dictionary, the default value is NO
let NSStringEncodingDetectionFromWindowsKey: NSString!

// NSString value; if this key is not present in the dictionary, the default value is U+FFFD
let NSStringEncodingDetectionLossySubstitutionKey: NSString!

// NSString value; ISO language code; if this key is not present in the dictionary, no such information is considered
let NSStringEncodingDetectionLikelyLanguageKey: NSString!

extension NSString {
    
    /* These methods are no longer recommended since they do not work with property lists and strings files in binary plist format. Please use the APIs in NSPropertyList.h instead.
    */
    func propertyList() -> AnyObject!
    func propertyListFromStringsFileFormat() -> [NSObject : AnyObject]!
}

extension NSString {
    
    /* The following methods are deprecated and will be removed from this header file in the near future. These methods use [NSString defaultCStringEncoding] as the encoding to convert to, which means the results depend on the user's language and potentially other settings. This might be appropriate in some cases, but often these methods are misused, resulting in issues when running in languages other then English. UTF8String in general is a much better choice when converting arbitrary NSStrings into 8-bit representations. Additional potential replacement methods are being introduced in NSString as appropriate.
    */
    
    /* This method is unsafe because it could potentially cause buffer overruns. You should use -getCharacters:range: instead.
    */
    func getCharacters(buffer: UnsafePointer<unichar>)
}

var NSProprietaryStringEncoding: Int { get } /* Installation-specific encoding */

/* The rest of this file is bookkeeping stuff that has to be here. Don't use this stuff, don't refer to it.
*/

class NSSimpleCString : NSString {
}

class NSConstantString : NSSimpleCString {
}

var _NSConstantStringClassReference: UnsafePointer<()>
