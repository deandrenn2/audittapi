import Select, { SingleValue } from "react-select";
import { useGuide } from "./useGuide";

export interface Option {
    value?: string;
    label?: string;
}


export const GuideSelect = ({ selectedValue, name, className, xChange, required, isSearchable, isDisabled }: { selectedValue?: Option, name?: string, className?: string, xChange: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryGuide, guides } = useGuide();

    const options: readonly Option[] | undefined = guides?.map((item) => ({
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
                isDisabled={queryGuide?.isLoading || isDisabled}
                isLoading={queryGuide?.isLoading}
                isClearable
                options={options}
            />
        )
}