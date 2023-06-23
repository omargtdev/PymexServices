namespace Pymex.Services.Mappers.Contracts
{
    /// <summary>
    ///     Interfaz para mapeos de operaciones genéricas, es decir, de una entidad a una data contractual, ya
    ///     sea para crear, actualizar o listar.
    /// </summary>
    /// <typeparam name="E">El tipo que será la entidad.</typeparam>
    /// <typeparam name="DC">El tipo que será la data contractual.</typeparam>
    public interface IGenericMapper<E, DC> : IDataContractMapper<E, DC> where E : class where DC : class
    {
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
