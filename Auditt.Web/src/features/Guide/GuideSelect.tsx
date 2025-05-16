import Select, { SingleValue } from "react-select";
import { useGuide } from "./useGuide";
import { useEffect, useState } from "react";
import useAssessmentContext from "../../shared/context/useAssessmentContext";

export interface Option {
    value?: string;
    label?: string;
}


export const GuideSelect = ({ name, className, xChange, required, isSearchable, isDisabled }: { name?: string, className?: string, xChange?: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryGuide, guides } = useGuide();
    const { setSelectedGuide: setGuide, selectedGuide: guide } = useAssessmentContext();

    const options: readonly Option[] | undefined = guides?.map((item) => ({
        value: item?.id?.toString(),
        label: isSearchable ? `${item.name}` : (item.name),
    }));
    const [selectedGuide, setSelectedGuide] = useState<Option | undefined>(() => ({
        value: guide?.toString(),
        label: guides?.find(x => x.id === guide)?.name ?? 'Seleccione una guía',
    }));
    useEffect(() => {
        if (guide && guides) {
            setSelectedGuide({
                value: guide?.toString(),
                label: guides?.find(x => x.id === guide)?.name ?? 'Seleccione una guía',
            });
        }
    }, [guide, guides, setSelectedGuide]);






    const handleChangeGuide = (newValue: SingleValue<Option>) => {
        setSelectedGuide({
            value: newValue?.value,
            label: newValue?.label,
        });
        setGuide(Number(newValue?.value));
        if (xChange) {
            xChange(newValue);
        }
    }


    if (options)
        return (
            <Select
                name={name || 'idClient'}
                className={className}
                value={selectedGuide}
                onChange={handleChangeGuide}
                required={required}
                loadingMessage={() => 'Cargando...'}
                isDisabled={queryGuide?.isLoading || isDisabled}
                isLoading={queryGuide?.isLoading}
                options={options}
            />
        )
}