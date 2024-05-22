using CombatGame.Commons.Combat;

namespace CombatGame.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        private float _previousFrameTime;
        private bool _alreadApllyForce;
        private readonly Attack _currentAttack;

        public PlayerAttackState(PlayerController playerStateMachine, int attackIndex) : base(playerStateMachine)
        {
            _currentAttack = playerStateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            context.WeaponHandler.EnableWeapon();
            context.WeaponDamage.SetDamage(_currentAttack.Damage, _currentAttack.KnockBack);
            context.EquippedWeapon.GetWeapon();
            context.Animator.CrossFadeInFixedTime(_currentAttack.AttackName, _currentAttack.TransitionDuration);
            SetCurrentAttack();
        }

        public override void Exit()
        {
            context.WeaponHandler.DisableWeapon();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();

            float normalizedTime = GetNormalizedTime(context.Animator, tag:"Attack");

            if (normalizedTime >= _previousFrameTime && normalizedTime < 1f)
            {
                if (normalizedTime >= _currentAttack.ForceTime)
                {
                    TryApplyForce();
                }

                if (context.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else
            {
                if (context.Targeter.CurrentTarget == null)
                {
                    context.SwitchState(new PlayerGroundedState(context));
                }
                else
                {
                    context.SwitchState(new PlayerTargetingState(context));
                }
            }

            _previousFrameTime = normalizedTime;
        }

        private void TryApplyForce()
        {
            if (_alreadApllyForce) return;

            context.ForceReceiver.AddForce(
                context.transform.forward * _currentAttack.Force
            );

            _alreadApllyForce = true;
        }

        private void TryComboAttack(float normalizedTime)
        {
            if (_currentAttack.ComboAttackTime == -1) return;

            if (normalizedTime < _currentAttack.ComboAttackTime) return;

            context.SwitchState(new PlayerAttackState(context, _currentAttack.ComboStateIndex));
        }

        private void SetCurrentAttack()
        {
            int currentAttack = context.CurrentAttack;
            int lastAttack = context.Attacks.Length - 1;

            if (currentAttack == lastAttack) 
            {
                currentAttack = 0;
            }
            else
            {
                currentAttack = context.CurrentAttack + 1;
            }

            context.CurrentAttack = currentAttack;
        }
    }
}