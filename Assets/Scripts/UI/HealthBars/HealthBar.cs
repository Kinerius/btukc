using Cysharp.Threading.Tasks;
using Game;
using Stats;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HealthBars
{
    public class HealthBar : PooledObject
    {
        [SerializeField] private Image currentHealthView;
        [SerializeField] private float timeVisible = 3;
        [SerializeField] private CanvasGroup canvasGroup;

        private Entity entity;
        private IReactiveStat<float> hp;
        private IReactiveStat<float> hpMax;
        private float currentHealth;
        private float totalHealth;
        private float targetHealth;

        private CancellationTokenSource cancellationTokenSource;
        private Transform healthBarAnchor;
        private UnityEngine.Camera camera;
        private RectTransform rectTransform;
        private GlobalClock clock;
        private float lastTimeHealthChanged;
        private bool isVisible = true;

        public void Setup(Entity entity, GlobalClock clock)
        {
            this.clock = clock;
            SetVisible(false);
            lastTimeHealthChanged = -timeVisible;

            // TODO: inject this
            camera = UnityEngine.Camera.main;
            rectTransform = (RectTransform)transform;

            this.entity = entity;

            healthBarAnchor = entity.GetView().GetAnchor("HealthBar");
            hp = entity.Stats.ObserveStat("HP");
            hpMax = entity.Stats.ObserveStat("HPMax");

            totalHealth = hpMax.Value;
            currentHealth = hp.Value;
            targetHealth = currentHealth;

            UpdateCurrentHealthView();

            hp.OnChanged += OnHealthChanged;

            cancellationTokenSource = new CancellationTokenSource();
            HealthUpdateAsync(cancellationTokenSource.Token).Forget();
        }

        public override void ReturnToPool()
        {
            CleanUp();
            base.ReturnToPool();
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            hp.OnChanged -= OnHealthChanged;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        private void OnHealthChanged(float newHealth)
        {
            targetHealth = newHealth;
            lastTimeHealthChanged = clock.time;
        }

        // TODO: Instead of having multiple of this running, consider the manager to run this just once and iterate all health bars
        private async UniTask HealthUpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    await UniTask.Yield();
                    cancellationToken.ThrowIfCancellationRequested();

                    if (clock.time - lastTimeHealthChanged > timeVisible)
                    {
                        SetVisible(false);
                        continue;
                    }
                    else
                    {
                        SetVisible(true);
                    }

                    UpdateCurrentHealthValues();
                    UpdatePosition();

                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) return;
                Debug.LogException(e);
            }
        }

        private void SetVisible(bool visible)
        {
            if (isVisible == visible) return;

            isVisible = visible;
            // todo: consider animating this value
            canvasGroup.alpha = isVisible ? 1 : 0;
        }

        private void UpdatePosition()
        {
            var targetPosition = camera.WorldToScreenPoint(healthBarAnchor.position);
            rectTransform.position = targetPosition;
        }

        private void UpdateCurrentHealthValues()
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, clock.deltaTime * 7);
            if (Math.Abs(currentHealth - targetHealth) > 0)
                UpdateCurrentHealthView();
        }

        private void UpdateCurrentHealthView()
        {
            currentHealthView.fillAmount = currentHealth / totalHealth;
        }
    }
}
