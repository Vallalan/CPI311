using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SkinnedModel;

namespace Common
{
    public class AnimatedObject : ModelObject
    {
        public AnimationPlayer AnimationPlayer { get; set; }
        protected SkinningData SkinningData { get; set; }

        public AnimatedObject()
        {
            AnimationPlayer = null;
        }

        private Model _model;
        public override Model Model
        {
            set
            {
                _model = value;
                SkinningData = _model.Tag as SkinningData;
                if (SkinningData == null)
                    return;
                AnimationPlayer = new AnimationPlayer(SkinningData);
            }

            get
            {
                return _model;
            }
        }

        public void StartAnimation(String name)
        {
            AnimationClip clip = SkinningData.AnimationClips[name];
            AnimationPlayer.StartClip(clip);
        }

        public void StopAnimation()
        {
            // Doing nothing right now.
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimationPlayer.Update(gameTime.ElapsedGameTime, true, World);
        }

        public override void Draw(Effect effect)
        {
            base.Draw(effect);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            Matrix[] bones = AnimationPlayer.GetSkinTransforms();
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
                mesh.Draw();
            }
        }
    }
}
