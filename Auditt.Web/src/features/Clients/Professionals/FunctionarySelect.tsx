import Select, { SingleValue } from "react-select";
import { useFunctionary } from "./UseFuntionary";

export interface Option {
    value?: string;
    label?: string;
}


export const FunctionarySelect = ({ selectedValue, name, className, xChange, required, isSearchable, isDisabled }: { selectedValue?: Option, name?: string, className?: string, xChange: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryFunctionary, functionarys } = useFunctionary();

    const options: readonly Option[] | undefined = functionarys?.map((item) => ({
        value: item?.id?.toString(),
        label: isSearchable ? `${item.firstName} ${item.lastName}` : (item.firstName + ' ' + item.lastName),
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
                isDisabled={queryFunctionary?.isLoading || isDisabled}
                isLoading={queryFunctionary?.isLoading}
                isClearable
                options={options}
            />
        )
}