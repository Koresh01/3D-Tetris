#import "MTRGUnityTracer.h"

@implementation MTRGUnityTracer

static BOOL _enabled;

+ (BOOL)enabled
{
	return _enabled;
}

+ (void)setEnabled:(BOOL)enabled
{
	_enabled = enabled;
}

@end

void mtrg_unity_tracer_i(NSString *format, ...)
{
	if (!format) return;

	NSString *finalFormat = [@"[myTarget.unity info] " stringByAppendingString:format];

	va_list args;
	va_start(args, format);
	NSLogv(finalFormat, args);
	va_end(args);
}

void mtrg_unity_tracer_d(NSString *format, ...)
{
	if (!MTRGUnityTracer.enabled) return;
	if (!format) return;

	NSString *finalFormat = [@"[myTarget.unity debug] " stringByAppendingString:format];

	va_list args;
	va_start(args, format);
	NSLogv(finalFormat, args);
	va_end(args);
}

void mtrg_unity_tracer_e(NSString *format, ...)
{
	if (!MTRGUnityTracer.enabled) return;
	if (!format) return;

	NSString *finalFormat = [@"[myTarget.unity error] " stringByAppendingString:format];

	va_list args;
	va_start(args, format);
	NSLogv(finalFormat, args);
	va_end(args);
}
