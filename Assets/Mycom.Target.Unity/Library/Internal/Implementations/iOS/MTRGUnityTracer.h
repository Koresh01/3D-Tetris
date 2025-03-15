#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface MTRGUnityTracer : NSObject

+ (void)setEnabled:(BOOL)enabled;

@end

extern void mtrg_unity_tracer_i(NSString *, ...);

extern void mtrg_unity_tracer_d(NSString *, ...);

extern void mtrg_unity_tracer_e(NSString *, ...);

NS_ASSUME_NONNULL_END