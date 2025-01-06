using FluentValidation;
using FestivalHoa.Properties.Interfaces.Core;

namespace FestivalHoa.Properties.Validation.Common; 

 /*   public class CommonValidation<TEntity, UEntityId> :  AbstractValidator<TEntity> where TEntity : class, IIdEntity<UEntityId> {
        public CommonValidation()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Tên không được rỗng.")
                .NotNull().WithMessage("Tên không được null.")
                .OverridePropertyName(x => x.Name);

        }
    }*/

    public class CommonValidation<TEntity, UEntityId> : AbstractValidator<TEntity> where TEntity : class, IIdEntity<UEntityId>
    {
        public CommonValidation()
        {
            
            RuleFor(entity => entity.Name)
                .NotEmpty().WithMessage("Tên danh mục không được rỗng.")
                .NotNull().WithMessage("Tên danh mục không được null.")
                .OverridePropertyName(x => x.Name);
            
            RuleFor(entity => entity.Code)
                .NotEmpty().WithMessage("Mã Code không được rỗng.")
                .NotNull().WithMessage("Mã Code không được null.")
                .OverridePropertyName(x => x.Code);
            
            RuleFor(entity => entity.CollectionName)
                .NotEmpty().WithMessage("CollectionName không được rỗng.")
                .NotNull().WithMessage("CollectionName không được null.")
                .OverridePropertyName(x => x.CollectionName);


            RuleFor(entity => entity.Sort)
                .NotNull().WithMessage("Thứ tự không được null.")
                .Must(x =>
                {
                    if (x >= 0 )
                    {
                        return true;
                    }

                    return false;
                }).WithMessage("Thứ tự phải là số và đủ lớn hơn 0 !")
                .OverridePropertyName(x => x.Sort);

        }
    }