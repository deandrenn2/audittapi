import { useScaleById } from "../useScales";


export const EquivalenceSelect = ({ idScale, selectedValue, name, className, xChange, required, isDisabled }: { idScale: number, selectedValue?: string, name?: string, className?: string, xChange?: (value: HTMLSelectElement) => void, required?: boolean, isDisabled?: boolean }) => {
    const { queryScale, scale: equivalences } = useScaleById(idScale);

    console.log(equivalences, "Equivalencias");

    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (xChange)
            xChange(e.target);
    }

    return (
        <select
            name={name || 'idBrand'}
            className={className || 'border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500 bg-white'}
            value={selectedValue}
            disabled={isDisabled}
            onChange={handleChange}
            required={required}
        >
            {queryScale?.isLoading && <option>Cargando...</option>}

            {queryScale?.isError && <option>Error</option>}
            {equivalences?.equivalences?.map((x) => (
                <option value={x.id} key={x.id}>{x.name}</option>
            ))}
        </select>
    )
}