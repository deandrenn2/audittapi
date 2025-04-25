import Select, { SingleValue } from "react-select";
import { useClient } from "./useClient";

export interface Option {
    value?: string;
    label?: string;
}


export const ClientSelect = ({ selectedValue, name, className, xChange, required, isSearchable, isDisabled }: { selectedValue?: Option, name?: string, className?: string, xChange: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryClients, clients } = useClient();

    const options: readonly Option[] | undefined = clients?.map((item) => ({
        value: item?.id?.toString(),
        label: isSearchable ? `${item.name} - ${item.nit} ` : (item.name + ' ' + item.nit),
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
                isDisabled={queryClients?.isLoading || isDisabled}
                isLoading={queryClients?.isLoading}
                isClearable
                options={options}
            />
        )
}