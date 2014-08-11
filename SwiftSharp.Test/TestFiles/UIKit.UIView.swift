import Foundation
import UIKit

//
//  UIView.h
//  UIKit
//
//  Copyright (c) 2005-2014 Apple Inc. All rights reserved.
//

enum UIViewAnimationCurve : Int {
    case EaseInOut // slow at beginning and end
    case EaseIn // slow at beginning
    case EaseOut // slow at end
    case Linear
}

enum UIViewContentMode : Int {
    case ScaleToFill
    case ScaleAspectFit // contents scaled to fit with fixed aspect. remainder is transparent
    case ScaleAspectFill // contents scaled to fill with fixed aspect. some portion of content may be clipped.
    case Redraw // redraw on bounds change (calls -setNeedsDisplay)
    case Center // contents remain same size. positioned adjusted.
    case Top
    case Bottom
    case Left
    case Right
    case TopLeft
    case TopRight
    case BottomLeft
    case BottomRight
}

enum UIViewAnimationTransition : Int {
    case None
    case FlipFromLeft
    case FlipFromRight
    case CurlUp
    case CurlDown
}

struct UIViewAutoresizing : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var None: UIViewAutoresizing { get }
    static var FlexibleLeftMargin: UIViewAutoresizing { get }
    static var FlexibleWidth: UIViewAutoresizing { get }
    static var FlexibleRightMargin: UIViewAutoresizing { get }
    static var FlexibleTopMargin: UIViewAutoresizing { get }
    static var FlexibleHeight: UIViewAutoresizing { get }
    static var FlexibleBottomMargin: UIViewAutoresizing { get }
}

struct UIViewAnimationOptions : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var LayoutSubviews: UIViewAnimationOptions { get }
    static var AllowUserInteraction: UIViewAnimationOptions { get } // turn on user interaction while animating
    static var BeginFromCurrentState: UIViewAnimationOptions { get } // start all views from current value, not initial value
    static var Repeat: UIViewAnimationOptions { get } // repeat animation indefinitely
    static var Autoreverse: UIViewAnimationOptions { get } // if repeat, run animation back and forth
    static var OverrideInheritedDuration: UIViewAnimationOptions { get } // ignore nested duration
    static var OverrideInheritedCurve: UIViewAnimationOptions { get } // ignore nested curve
    static var AllowAnimatedContent: UIViewAnimationOptions { get } // animate contents (applies to transitions only)
    static var ShowHideTransitionViews: UIViewAnimationOptions { get } // flip to/from hidden state instead of adding/removing
    static var OverrideInheritedOptions: UIViewAnimationOptions { get } // do not inherit any options or animation type
    
    static var CurveEaseInOut: UIViewAnimationOptions { get } // default
    static var CurveEaseIn: UIViewAnimationOptions { get }
    static var CurveEaseOut: UIViewAnimationOptions { get }
    static var CurveLinear: UIViewAnimationOptions { get }
    
    static var TransitionNone: UIViewAnimationOptions { get } // default
    static var TransitionFlipFromLeft: UIViewAnimationOptions { get }
    static var TransitionFlipFromRight: UIViewAnimationOptions { get }
    static var TransitionCurlUp: UIViewAnimationOptions { get }
    static var TransitionCurlDown: UIViewAnimationOptions { get }
    static var TransitionCrossDissolve: UIViewAnimationOptions { get }
    static var TransitionFlipFromTop: UIViewAnimationOptions { get }
    static var TransitionFlipFromBottom: UIViewAnimationOptions { get }
}

struct UIViewKeyframeAnimationOptions : RawOptionSet {
    init(_ value: UInt)
    var value: UInt
    static var LayoutSubviews: UIViewKeyframeAnimationOptions { get }
    static var AllowUserInteraction: UIViewKeyframeAnimationOptions { get } // turn on user interaction while animating
    static var BeginFromCurrentState: UIViewKeyframeAnimationOptions { get } // start all views from current value, not initial value
    static var Repeat: UIViewKeyframeAnimationOptions { get } // repeat animation indefinitely
    static var Autoreverse: UIViewKeyframeAnimationOptions { get } // if repeat, run animation back and forth
    static var OverrideInheritedDuration: UIViewKeyframeAnimationOptions { get } // ignore nested duration
    static var OverrideInheritedOptions: UIViewKeyframeAnimationOptions { get } // do not inherit any options or animation type
    
    static var CalculationModeLinear: UIViewKeyframeAnimationOptions { get } // default
    static var CalculationModeDiscrete: UIViewKeyframeAnimationOptions { get }
    static var CalculationModePaced: UIViewKeyframeAnimationOptions { get }
    static var CalculationModeCubic: UIViewKeyframeAnimationOptions { get }
    static var CalculationModeCubicPaced: UIViewKeyframeAnimationOptions { get }
}

enum UISystemAnimation : UInt {
    case Delete // removes the views from the hierarchy when complete
}

enum UIViewTintAdjustmentMode : Int {
    case Automatic
    
    case Normal
    case Dimmed
}

protocol UICoordinateSpace : NSObjectProtocol {
    
    func convertPoint(point: CGPoint, toCoordinateSpace coordinateSpace: UICoordinateSpace!) -> CGPoint
    func convertPoint(point: CGPoint, fromCoordinateSpace coordinateSpace: UICoordinateSpace!) -> CGPoint
    func convertRect(rect: CGRect, toCoordinateSpace coordinateSpace: UICoordinateSpace!) -> CGRect
    func convertRect(rect: CGRect, fromCoordinateSpace coordinateSpace: UICoordinateSpace!) -> CGRect
    
    var bounds: CGRect { get }
}

/*!
 UITabBarController manages a button bar and transition view, for an application with multiple top-level modes.
 
 To use in your application, add its view to the view hierarchy, then add top-level view controllers in order.
 Most clients will not need to subclass UITabBarController.

 If more than five view controllers are added to a tab bar controller, only the first four will display.
 The rest will be accessible under an automatically generated More item.
 
 UITabBarController is rotatable if all of its view controllers are rotatable.
 */
class UIView : UIResponder, NSCoding, UIAppearance, NSObjectProtocol, UIAppearanceContainer, UIDynamicItem, UITraitEnvironment, UICoordinateSpace {
    
    class func layerClass() -> AnyClass! // default is [CALayer class]. Used when creating the underlying layer for the view.
    
    init(frame: CGRect) // default initializer
    
    var userInteractionEnabled: Bool // default is YES. if set to NO, user events (touch, keys) are ignored and removed from the event queue.
    var tag: Int // default is 0
    var layer: CALayer! { get } // returns view's layer. Will always return a non-nil value. view is layer's delegate
}

extension UIView : Reflectable {
    func getMirror() -> Mirror
}

extension UIView {
    
    // animatable. do not use frame if view is transformed since it will not correctly reflect the actual location of the view. use bounds + center instead.
    var frame: CGRect
    
    // use bounds/center and not frame if non-identity transform. if bounds dimension is odd, center may be have fractional part
    var bounds: CGRect // default bounds is zero origin, frame size. animatable
    var center: CGPoint // center is center of frame. animatable
    var transform: CGAffineTransform // default is CGAffineTransformIdentity. animatable
    var contentScaleFactor: CGFloat
    
    var multipleTouchEnabled: Bool // default is NO
    var exclusiveTouch: Bool // default is NO
    
    func hitTest(point: CGPoint, withEvent event: UIEvent!) -> UIView! // recursively calls -pointInside:withEvent:. point is in the receiver's coordinate system
    func pointInside(point: CGPoint, withEvent event: UIEvent!) -> Bool // default returns YES if point is in bounds
    
    func convertPoint(point: CGPoint, toView view: UIView!) -> CGPoint
    func convertPoint(point: CGPoint, fromView view: UIView!) -> CGPoint
    func convertRect(rect: CGRect, toView view: UIView!) -> CGRect
    func convertRect(rect: CGRect, fromView view: UIView!) -> CGRect
    
    var autoresizesSubviews: Bool // default is YES. if set, subviews are adjusted according to their autoresizingMask if self.bounds changes
    var autoresizingMask: UIViewAutoresizing // simple resize. default is UIViewAutoresizingNone
    
    func sizeThatFits(size: CGSize) -> CGSize // return 'best' size to fit given size. does not actually resize view. Default is return existing view size
    func sizeToFit() // calls sizeThatFits: with current view bounds and changes bounds size.
}

extension UIView {
    
    var superview: UIView! { get }
    var subviews: [AnyObject]! { get }
    var window: UIWindow! { get }
    
    func removeFromSuperview()
    func insertSubview(view: UIView!, atIndex index: Int)
    func exchangeSubviewAtIndex(index1: Int, withSubviewAtIndex index2: Int)
    
    func addSubview(view: UIView!)
    func insertSubview(view: UIView!, belowSubview siblingSubview: UIView!)
    func insertSubview(view: UIView!, aboveSubview siblingSubview: UIView!)
    
    func bringSubviewToFront(view: UIView!)
    func sendSubviewToBack(view: UIView!)
    
    func didAddSubview(subview: UIView!)
    func willRemoveSubview(subview: UIView!)
    
    func willMoveToSuperview(newSuperview: UIView!)
    func didMoveToSuperview()
    func willMoveToWindow(newWindow: UIWindow!)
    func didMoveToWindow()
    
    func isDescendantOfView(view: UIView!) -> Bool // returns YES for self.
    func viewWithTag(tag: Int) -> UIView! // recursive search. includes self
    
    // Allows you to perform layout before the drawing cycle happens. -layoutIfNeeded forces layout early
    func setNeedsLayout()
    func layoutIfNeeded()
    
    func layoutSubviews() // override point. called by layoutIfNeeded automatically. As of iOS 6.0, when constraints-based layout is used the base implementation applies the constraints-based layout, otherwise it does nothing.
    
    /*
     -layoutMargins returns a set of insets from the edge of the view's bounds that denote a default spacing for laying out content.
     If preservesSuperviewLayoutMargins is YES, margins cascade down the view tree, adjusting for geometry offsets, so that setting the left value of layoutMargins on a superview will affect the left value of layoutMargins for subviews positioned close to the left edge of their superview's bounds
     If your view subclass uses layoutMargins in its layout or drawing, override -layoutMarginsDidChange in order to refresh your view if the margins change.
     */
    var layoutMargins: UIEdgeInsets
    var preservesSuperviewLayoutMargins: Bool // default is NO - set to enable pass-through or cascading behavior of margins from this view’s parent to its children
    func layoutMarginsDidChange()
}

extension UIView {
    
    func drawRect(rect: CGRect)
    
    func setNeedsDisplay()
    func setNeedsDisplayInRect(rect: CGRect)
    
    var clipsToBounds: Bool // When YES, content and subviews are clipped to the bounds of the view. Default is NO.
    var backgroundColor: UIColor! // default is nil. Can be useful with the appearance proxy on custom UIView subclasses.
    var alpha: CGFloat // animatable. default is 1.0
    var opaque: Bool // default is YES. opaque views must fill their entire bounds or the results are undefined. the active CGContext in drawRect: will not have been cleared and may have non-zeroed pixels
    var clearsContextBeforeDrawing: Bool // default is YES. ignored for opaque views. for non-opaque views causes the active CGContext in drawRect: to be pre-filled with transparent pixels
    var hidden: Bool // default is NO. doesn't check superviews
    var contentMode: UIViewContentMode // default is UIViewContentModeScaleToFill
    // animatable. default is unit rectangle {{0,0} {1,1}}. Now deprecated: please use -[UIImage resizableImageWithCapInsets:] to achieve the same effect.
    
    var maskView: UIView!
    
    /*
     -tintColor always returns a color. The color returned is the first non-default value in the receiver's superview chain (starting with itself).
     If no non-default value is found, a system-defined color is returned.
     If this view's -tintAdjustmentMode returns Dimmed, then the color that is returned for -tintColor will automatically be dimmed.
     If your view subclass uses tintColor in its rendering, override -tintColorDidChange in order to refresh the rendering if the color changes.
     */
    var tintColor: UIColor!
    
    /*
     -tintAdjustmentMode always returns either UIViewTintAdjustmentModeNormal or UIViewTintAdjustmentModeDimmed. The value returned is the first non-default value in the receiver's superview chain (starting with itself).
     If no non-default value is found, UIViewTintAdjustmentModeNormal is returned.
     When tintAdjustmentMode has a value of UIViewTintAdjustmentModeDimmed for a view, the color it returns from tintColor will be modified to give a dimmed appearance.
     When the tintAdjustmentMode of a view changes (either the view's value changing or by one of its superview's values changing), -tintColorDidChange will be called to allow the view to refresh its rendering.
     */
    var tintAdjustmentMode: UIViewTintAdjustmentMode
    
    /*
     The -tintColorDidChange message is sent to appropriate subviews of a view when its tintColor is changed by client code or to subviews in the view hierarchy of a view whose tintColor is implicitly changed when its superview or tintAdjustmentMode changes.
     */
    func tintColorDidChange()
}

extension UIView {
    
    class func beginAnimations(animationID: String!, context: UnsafePointer<()>) // additional context info passed to will start/did stop selectors. begin/commit can be nested
    class func commitAnimations() // starts up any animations when the top level animation is commited
    
    // no getters. if called outside animation block, these setters have no effect.
    class func setAnimationDelegate(delegate: AnyObject!) // default = nil
    class func setAnimationWillStartSelector(selector: Selector) // default = NULL. -animationWillStart:(NSString *)animationID context:(void *)context
    class func setAnimationDidStopSelector(selector: Selector) // default = NULL. -animationDidStop:(NSString *)animationID finished:(NSNumber *)finished context:(void *)context
    class func setAnimationDuration(duration: NSTimeInterval) // default = 0.2
    class func setAnimationDelay(delay: NSTimeInterval) // default = 0.0
    class func setAnimationStartDate(startDate: NSDate!) // default = now ([NSDate date])
    class func setAnimationCurve(curve: UIViewAnimationCurve) // default = UIViewAnimationCurveEaseInOut
    class func setAnimationRepeatCount(repeatCount: Float) // default = 0.0.  May be fractional
    class func setAnimationRepeatAutoreverses(repeatAutoreverses: Bool) // default = NO. used if repeat count is non-zero
    class func setAnimationBeginsFromCurrentState(fromCurrentState: Bool) // default = NO. If YES, the current view position is always used for new animations -- allowing animations to "pile up" on each other. Otherwise, the last end state is used for the animation (the default).
    
    class func setAnimationTransition(transition: UIViewAnimationTransition, forView view: UIView!, cache: Bool) // current limitation - only one per begin/commit block
    
    class func setAnimationsEnabled(enabled: Bool) // ignore any attribute changes while set.
    class func areAnimationsEnabled() -> Bool
    class func performWithoutAnimation(actionsWithoutAnimation: (() -> Void)!)
}

extension UIView {
    
    class func animateWithDuration(duration: NSTimeInterval, delay: NSTimeInterval, options: UIViewAnimationOptions, animations: (() -> Void)!, completion: ((Bool) -> Void)!)
    
    class func animateWithDuration(duration: NSTimeInterval, animations: (() -> Void)!, completion: ((Bool) -> Void)!) // delay = 0.0, options = 0
    
    class func animateWithDuration(duration: NSTimeInterval, animations: (() -> Void)!) // delay = 0.0, options = 0, completion = NULL
    
    /* Performs `animations` using a timing curve described by the motion of a spring. When `dampingRatio` is 1, the animation will smoothly decelerate to its final model values without oscillating. Damping ratios less than 1 will oscillate more and more before coming to a complete stop. You can use the initial spring velocity to specify how fast the object at the end of the simulated spring was moving before it was attached. It's a unit coordinate system, where 1 is defined as travelling the total animation distance in a second. So if you're changing an object's position by 200pt in this animation, and you want the animation to behave as if the object was moving at 100pt/s before the animation started, you'd pass 0.5. You'll typically want to pass 0 for the velocity. */
    class func animateWithDuration(duration: NSTimeInterval, delay: NSTimeInterval, usingSpringWithDamping dampingRatio: CGFloat, initialSpringVelocity velocity: CGFloat, options: UIViewAnimationOptions, animations: (() -> Void)!, completion: ((Bool) -> Void)!)
    
    class func transitionWithView(view: UIView!, duration: NSTimeInterval, options: UIViewAnimationOptions, animations: (() -> Void)!, completion: ((Bool) -> Void)!)
    
    class func transitionFromView(fromView: UIView!, toView: UIView!, duration: NSTimeInterval, options: UIViewAnimationOptions, completion: ((Bool) -> Void)!) // toView added to fromView.superview, fromView removed from its superview
    
    /* Performs the requested system-provided animation on one or more views. Specify addtional animations in the parallelAnimations block. These additional animations will run alongside the system animation with the same timing and duration that the system animation defines/inherits. Additional animations should not modify properties of the view on which the system animation is being performed. Not all system animations honor all available options.
     */
    class func performSystemAnimation(animation: UISystemAnimation, onViews views: [AnyObject]!, options: UIViewAnimationOptions, animations parallelAnimations: (() -> Void)!, completion: ((Bool) -> Void)!)
}

extension UIView {
    
    class func animateKeyframesWithDuration(duration: NSTimeInterval, delay: NSTimeInterval, options: UIViewKeyframeAnimationOptions, animations: (() -> Void)!, completion: ((Bool) -> Void)!)
    class func addKeyframeWithRelativeStartTime(frameStartTime: Double, relativeDuration frameDuration: Double, animations: (() -> Void)!) // start time and duration are values between 0.0 and 1.0 specifying time and duration relative to the overall time of the keyframe animation
}

extension UIView {
    
    var gestureRecognizers: [AnyObject]!
    
    func addGestureRecognizer(gestureRecognizer: UIGestureRecognizer!)
    func removeGestureRecognizer(gestureRecognizer: UIGestureRecognizer!)
    
    // called when the recognizer attempts to transition out of UIGestureRecognizerStatePossible if a touch hit-tested to this view will be cancelled as a result of gesture recognition
    // returns YES by default. return NO to cause the gesture recognizer to transition to UIGestureRecognizerStateFailed
    // subclasses may override to prevent recognition of particular gestures. for example, UISlider prevents swipes parallel to the slider that start in the thumb
    func gestureRecognizerShouldBegin(gestureRecognizer: UIGestureRecognizer!) -> Bool
}

extension UIView {
    
    /*! Begins applying `effect` to the receiver. The effect's emitted keyPath/value pairs will be
        applied to the view's presentation layer.
     
        Animates the transition to the motion effect's values using the present UIView animation
        context. */
    func addMotionEffect(effect: UIMotionEffect!)
    
    /*! Stops applying `effect` to the receiver. Any affected presentation values will animate to
        their post-removal values using the present UIView animation context. */
    func removeMotionEffect(effect: UIMotionEffect!)
    
    var motionEffects: [AnyObject]!
}

//
// UIView Constraint-based Layout Support
//

enum UILayoutConstraintAxis : Int {
    case Horizontal
    case Vertical
}

// Installing Constraints

/* A constraint is typically installed on the closest common ancestor of the views involved in the constraint. 
 It is required that a constraint be installed on _a_ common ancestor of every view involved.  The numbers in a constraint are interpreted in the coordinate system of the view it is installed on.  A view is considered to be an ancestor of itself.
 */
extension UIView {
    
    func constraints() -> [AnyObject]!
    
    func addConstraint(constraint: NSLayoutConstraint!) // This method will be deprecated in a future release and should be avoided.  Instead, set NSLayoutConstraint's active property to YES.
    func addConstraints(constraints: [AnyObject]!) // This method will be deprecated in a future release and should be avoided.  Instead use +[NSLayoutConstraint activateConstraints:].
    func removeConstraint(constraint: NSLayoutConstraint!) // This method will be deprecated in a future release and should be avoided.  Instead set NSLayoutConstraint's active property to NO.
    func removeConstraints(constraints: [AnyObject]!) // This method will be deprecated in a future release and should be avoided.  Instead use +[NSLayoutConstraint deactivateConstraints:].
}

// Core Layout Methods

/* To render a window, the following passes will occur, if necessary.  
 
 update constraints
 layout
 display
 
 Please see the conceptual documentation for a discussion of these methods.
 */

extension UIView {
    func updateConstraintsIfNeeded() // Updates the constraints from the bottom up for the view hierarchy rooted at the receiver. UIWindow's implementation creates a layout engine if necessary first.
    func updateConstraints() // Override this to adjust your special constraints during a constraints update pass
    func needsUpdateConstraints() -> Bool
    func setNeedsUpdateConstraints()
}

// Compatibility and Adoption

extension UIView {
    
    /* by default, the autoresizing mask on a view gives rise to constraints that fully determine the view's position.  Any constraints you set on the view are likely to conflict with autoresizing constraints, so you must turn off this property first. IB will turn it off for you.
     */
    func translatesAutoresizingMaskIntoConstraints() -> Bool // Default YES
    func setTranslatesAutoresizingMaskIntoConstraints(flag: Bool)
    
    /* constraint-based layout engages lazily when someone tries to use it (e.g., adds a constraint to a view).  If you do all of your constraint set up in -updateConstraints, you might never even receive updateConstraints if no one makes a constraint.  To fix this chicken and egg problem, override this method to return YES if your view needs the window to use constraint-based layout.  
     */
    class func requiresConstraintBasedLayout() -> Bool
}

// Separation of Concerns

extension UIView {
    
    /* Constraints do not actually relate the frames of the views, rather they relate the "alignment rects" of views.  This is the same as the frame unless overridden by a subclass of UIView.  Alignment rects are the same as the "layout rects" shown in Interface Builder 3.  Typically the alignment rect of a view is what the end user would think of as the bounding rect around a control, omitting ornamentation like shadows and engraving lines.  The edges of the alignment rect are what is interesting to align, not the shadows and such.  
     */
    
    /* These two methods should be inverses of each other.  UIKit will call both as part of layout computation.
     They may be overridden to provide arbitrary transforms between frame and alignment rect, though the two methods must be inverses of each other.
     However, the default implementation uses -alignmentRectInsets, so just override that if it's applicable.  It's easier to get right. 
     A view that displayed an image with some ornament would typically override these, because the ornamental part of an image would scale up with the size of the frame.  
     Set the NSUserDefault UIViewShowAlignmentRects to YES to see alignment rects drawn.
     */
    func alignmentRectForFrame(frame: CGRect) -> CGRect
    func frameForAlignmentRect(alignmentRect: CGRect) -> CGRect
    
    /* override this if the alignment rect is obtained from the frame by insetting each edge by a fixed amount.  This is only called by alignmentRectForFrame: and frameForAlignmentRect:.
     */
    func alignmentRectInsets() -> UIEdgeInsets
    
    /* When you make a constraint on the NSLayoutAttributeBaseline of a view, the system aligns with the bottom of the view returned from this method. A nil return is interpreted as the receiver, and a non-nil return must be in the receiver's subtree.  UIView's implementation returns self.
     */
    func viewForBaselineLayout() -> UIView!
    
    /* Override this method to tell the layout system that there is something it doesn't natively understand in this view, and this is how large it intrinsically is.  A typical example would be a single line text field.  The layout system does not understand text - it must just be told that there's something in the view, and that that something will take a certain amount of space if not clipped.  
     
     In response, UIKit will set up constraints that specify (1) that the opaque content should not be compressed or clipped, (2) that the view prefers to hug tightly to its content. 
     
     A user of a view may need to specify the priority of these constraints.  For example, by default, a push button 
     -strongly wants to hug its content in the vertical direction (buttons really ought to be their natural height)
     -weakly hugs its content horizontally (extra side padding between the title and the edge of the bezel is acceptable)
     -strongly resists compressing or clipping content in both directions. 
     
     However, you might have a case where you'd prefer to show all the available buttons with truncated text rather than losing some of the buttons. The truncation might only happen in portrait orientation but not in landscape, for example. In that case you'd want to setContentCompressionResistancePriority:forAxis: to (say) UILayoutPriorityDefaultLow for the horizontal axis.
     
     The default 'strong' and 'weak' priorities referred to above are UILayoutPriorityDefaultHigh and UILayoutPriorityDefaultLow.  
     
     Note that not all views have an intrinsicContentSize.  UIView's default implementation is to return (UIViewNoIntrinsicMetric, UIViewNoIntrinsicMetric).  The _intrinsic_ content size is concerned only with data that is in the view itself, not in other views. Remember that you can also set constant width or height constraints on any view, and you don't need to override instrinsicContentSize if these dimensions won't be changing with changing view content.
     */
    // -1
    func intrinsicContentSize() -> CGSize
    func invalidateIntrinsicContentSize() // call this when something changes that affects the intrinsicContentSize.  Otherwise UIKit won't notice that it changed.  
    
    func contentHuggingPriorityForAxis(axis: UILayoutConstraintAxis) -> UILayoutPriority
    func setContentHuggingPriority(priority: UILayoutPriority, forAxis axis: UILayoutConstraintAxis)
    
    func contentCompressionResistancePriorityForAxis(axis: UILayoutConstraintAxis) -> UILayoutPriority
    func setContentCompressionResistancePriority(priority: UILayoutPriority, forAxis axis: UILayoutConstraintAxis)
}
let UIViewNoIntrinsicMetric: CGFloat

// Size To Fit

let UILayoutFittingCompressedSize: CGSize
let UILayoutFittingExpandedSize: CGSize

extension UIView {
    /* The size fitting most closely to targetSize in which the receiver's subtree can be laid out while optimally satisfying the constraints. If you want the smallest possible size, pass UILayoutFittingCompressedSize; for the largest possible size, pass UILayoutFittingExpandedSize.
     Also see the comment for UILayoutPriorityFittingSizeLevel.
     */
    func systemLayoutSizeFittingSize(targetSize: CGSize) -> CGSize // Equivalent to sending -systemLayoutSizeFittingSize:withHorizontalFittingPriority:verticalFittingPriority: with UILayoutPriorityFittingSizeLevel for both priorities.
    func systemLayoutSizeFittingSize(targetSize: CGSize, withHorizontalFittingPriority horizontalFittingPriority: UILayoutPriority, verticalFittingPriority: UILayoutPriority) -> CGSize
}

// Debugging

/* Everything in this section should be used in debugging only, never in shipping code.  These methods may not exist in the future - no promises.  
 */
extension UIView {
    
    /* This returns a list of all the constraints that are affecting the current location of the receiver.  The constraints do not necessarily involve the receiver, they may affect the frame indirectly.
     Pass UILayoutConstraintAxisHorizontal for the constraints affecting [self center].x and CGRectGetWidth([self bounds]), and UILayoutConstraintAxisVertical for the constraints affecting[self center].y and CGRectGetHeight([self bounds]).
     */
    func constraintsAffectingLayoutForAxis(axis: UILayoutConstraintAxis) -> [AnyObject]!
    
    /* If there aren't enough constraints in the system to uniquely determine layout, we say the layout is ambiguous.  For example, if the only constraint in the system was x = y + 100, then there are lots of different possible values for x and y.  This situation is not automatically detected by UIKit, due to performance considerations and details of the algorithm used for layout.  
     The symptom of ambiguity is that views sometimes jump from place to place, or possibly are just in the wrong place.
     -hasAmbiguousLayout runs a check for whether there is another center and bounds the receiver could have that could also satisfy the constraints.
     -exerciseAmbiguousLayout does more.  It randomly changes the view layout to a different valid layout.  Making the UI jump back and forth can be helpful for figuring out where you're missing a constraint.  
     */
    func hasAmbiguousLayout() -> Bool
    func exerciseAmbiguityInLayout()
}

extension UIView {
    var restorationIdentifier: String!
    func encodeRestorableStateWithCoder(coder: NSCoder!)
    func decodeRestorableStateWithCoder(coder: NSCoder!)
}

extension UIView {
    /* 
    * When requesting a snapshot, 'afterUpdates' defines whether the snapshot is representative of what's currently on screen or if you wish to include any recent changes before taking the snapshot. 
     
     If called during layout from a committing transaction, snapshots occurring after the screen updates will include all changes made, regardless of when the snapshot is taken and the changes are made. For example:
     
         - (void)layoutSubviews {
             UIView *snapshot = [self snapshotViewAfterScreenUpdates:YES];
             self.alpha = 0.0;
         }
     
     The snapshot will appear to be empty since the change in alpha will be captured by the snapshot. If you need to animate the view during layout, animate the snapshot instead.
    
    * Creating snapshots from existing snapshots (as a method to duplicate, crop or create a resizable variant) is supported. In cases where many snapshots are needed, creating a snapshot from a common superview and making subsequent snapshots from it can be more performant. Please keep in mind that if 'afterUpdates' is YES, the original snapshot is committed and any changes made to it, not the view originally snapshotted, will be included.
     */
    func snapshotViewAfterScreenUpdates(afterUpdates: Bool) -> UIView!
    func resizableSnapshotViewFromRect(rect: CGRect, afterScreenUpdates afterUpdates: Bool, withCapInsets capInsets: UIEdgeInsets) -> UIView! // Resizable snapshots will default to stretching the center
    // Use this method to render a snapshot of the view hierarchy into the current context. Returns NO if the snapshot is missing image data, YES if the snapshot is complete. Calling this method from layoutSubviews while the current transaction is committing will capture what is currently displayed regardless if afterUpdates is YES.
    func drawViewHierarchyInRect(rect: CGRect, afterScreenUpdates afterUpdates: Bool) -> Bool
}

