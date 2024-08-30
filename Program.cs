namespace Cleanup
{
    internal class ProgramOneLiners
    {
        private const double TargetChangeTime = 1;

        private double _previousTargetSetTime;
        private bool _isTargetSet => _target != null; // only works if TrySetActiveTargetFromQuantum does not require _isTargetSet
        private object _lockedCandidateTarget;
        private object _lockedTarget;
        private object _target;
        private object _previousTarget;
        private object _activeTarget;
        private object _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            try
            {
                _lockedCandidateTarget = _lockedCandidateTarget?.CanBeTarget == true ? _lockedCandidateTarget : null;
                _lockedTarget = _lockedTarget?.CanBeTarget == true ? _lockedTarget : null;
                _previousTarget = _target;

				// Sets _activeTarget field
                TrySetActiveTargetFromQuantum(frame);

                // If target exists and can be targeted, it should stay within Target Change Time since last target change
                if (_target && _target.CanBeTarget && Time.time - _previousTargetSetTime < TargetChangeTime) return;

                if (_lockedTarget?.CanBeTarget == true) _target = _lockedTarget;
                else if (_activeTarget?.CanBeTarget == true) _target = _activeTarget;
                else _target = _targetInRangeContainer.GetTarget();
            }
            finally
            {
                if (_isTargetSet && _previousTarget != _target) _previousTargetSetTime = Time.time;
                TargetableEntity.Selected = _target;
            }
        }
    }

    internal class ProgramWithBraces
    {
        private const double TargetChangeTime = 1;

        private double _previousTargetSetTime;
        private bool _isTargetSet => _target != null; // only works if TrySetActiveTargetFromQuantum does not require _isTargetSet
        private object _lockedCandidateTarget;
        private object _lockedTarget;
        private object _target;
        private object _previousTarget;
        private object _activeTarget;
        private object _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            try
            {
                _lockedCandidateTarget = _lockedCandidateTarget?.CanBeTarget == true ? _lockedCandidateTarget : null;
                _lockedTarget = _lockedTarget?.CanBeTarget == true ? _lockedTarget : null;
                _previousTarget = _target;

				// Sets _activeTarget field
                TrySetActiveTargetFromQuantum(frame);

                // If target exists and can be targeted, it should stay within Target Change Time since last target change
                if (_target && _target.CanBeTarget && Time.time - _previousTargetSetTime < TargetChangeTime)
                {
                    return;
                }

                if (_lockedTarget?.CanBeTarget == true)
                {
                    _target = _lockedTarget;
                }
                else if (_activeTarget?.CanBeTarget == true)
                {
                    _target = _activeTarget;
                }
                else
                {
                    _target = _targetInRangeContainer.GetTarget();
                }
            }
            finally
            {
                if (_isTargetSet && _previousTarget != _target)
                {
                    _previousTargetSetTime = Time.time;
                }
                TargetableEntity.Selected = _target;
            }
        }
    }

    internal class ProgramSafe
    {
        private const double TargetChangeTime = 1;

        private double _previousTargetSetTime;
        private bool _isTargetSet;
        private object _lockedCandidateTarget;
        private object _lockedTarget;
        private object _target;
        private object _previousTarget;
        private object _activeTarget;
        private object _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            try
            {
                _lockedCandidateTarget = _lockedCandidateTarget?.CanBeTarget == true ? _lockedCandidateTarget : null;
                _lockedTarget = _lockedTarget?.CanBeTarget == true ? _lockedTarget : null;
                _previousTarget = _target;

                _isTargetSet = false;
				// Sets _activeTarget field
                TrySetActiveTargetFromQuantum(frame);

                // If target exists and can be targeted, it should stay within Target Change Time since last target change
                if (_target && _target.CanBeTarget && Time.time - _previousTargetSetTime < TargetChangeTime)
                {
                    _isTargetSet = true;
                    return;
                }

                if (_lockedTarget?.CanBeTarget == true)
                {
                    _isTargetSet = true;
                    _target = _lockedTarget;
                }
                else if (_activeTarget?.CanBeTarget == true)
                {
                    _isTargetSet = true;
                    _target = _activeTarget;
                }
                else
                {
                    _target = _targetInRangeContainer.GetTarget();
                    _isTargetSet = _target != null;
                }
            }
            finally
            {
                if (_isTargetSet && _previousTarget != _target)
                {
                    _previousTargetSetTime = Time.time;
                }
                TargetableEntity.Selected = _target;
            }
        }
    }
}
