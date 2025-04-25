import Select, { SingleValue } from "react-select";
import { useDataCuts } from "./useDataCuts";

export interface Option {
    value?: string;
    label?: string;
}


export const DataCutSelect = ({ selectedValue, name, className, xChange, required, isSearchable, isDisabled }: { selectedValue?: Option, name?: string, className?: string, xChange: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryDataCuts, dataCuts } = useDataCuts();

    const options: readonly Option[] | undefined = dataCuts?.map((item) => ({
        value: item?.id?.toString(),
        label: isSearchable ? `${item.name}` : (item.name),
    }));


    if (options)
        return (
            <Select
                name={name || 'idClient'}
                className={className}
                value={selectedValue}
                onChange={xChange}
                required={required}
                loadingMessage={() => 'Cargando...'}
                isDisabled={queryDataCuts?.isLoading || isDisabled}
                isLoading={queryDataCuts?.isLoading}
                isClearable
                options={options}
            />
        )
}