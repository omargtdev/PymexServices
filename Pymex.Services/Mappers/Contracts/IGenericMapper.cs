namespace Pymex.Services.Mappers.Contracts
{
    public interface IGenericMapper<E, DC> where E : class where DC : class
    {
        /// <summary>
        ///     Recibe la entidad y lo mapea para crear una data contractual.
        /// </summary>
        /// <param name="entity">La entidad con los valores a mapear.</param>
        /// <returns>La data contractual mapeada.</returns>
        DC ToDataContract(E entity);

        /// <summary>
        ///     Recibe la data contractual y lo mapea para crear una entidad.
        /// </summary>
        /// <param name="dataContract">El objeto contractual con los valores a mapear.</param>
        /// <returns>La entidad mapeada.</returns>
        E ToCreateEntity(DC dataContract);

        /// <summary>
        ///     Recibe la entidad y lo mapea para una actualización.
        /// </summary>
        /// <param name="entity">El valor de referencia de la entidad a actualizar.</param>
        /// <param name="dataContract">El objeto contractual con los valores a mapear.</param>
        void ToEditEntity(E entity, DC dataContract);
    }
}
