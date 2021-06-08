using UnityEngine;

/// IFollower is an interface that describes an object that
// can conditionally follow a target using a custom strategy
public interface IFollower {
    void SetTarget(Transform target);
    void FollowStart();
    void FollowStop();
    bool IsFollowing();
}
