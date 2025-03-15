#import "PlatformUtils.h"

UIViewController *mtrg_unity_topViewController()
{
    UIViewController *topViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
    while (topViewController.presentedViewController)
    {
        topViewController = topViewController.presentedViewController;
    }
    return topViewController;
}

void MTRGDebugLog(const char *message)
{
    if (message)
    {
        NSString *messageString = [NSString stringWithUTF8String:message];
        if (messageString)
        {
            NSLog(@"%@", messageString);
        }
    }
}
