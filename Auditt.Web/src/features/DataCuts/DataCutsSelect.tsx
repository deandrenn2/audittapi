import Select, { SingleValue } from "react-select";
import { useDataCuts } from "./useDataCuts";
import useAssessmentContext from "../../shared/context/useAssessmentContext";
import { useEffect, useState } from "react";

export interface Option {
    value?: string;
    label?: string;
}


export const DataCutSelect = ({ name, className, xChange, required, isSearchable, isDisabled }: { name?: string, className?: string, xChange?: (newValue: SingleValue<Option>) => void, required?: boolean, isSearchable?: boolean, isDisabled?: boolean }) => {
    const { queryDataCuts, dataCuts } = useDataCuts();
    const { setSelectedDataCut: setDataCut, selectedDataCut: dataCut } = useAssessmentContext();
    const options: readonly Option[] | undefined = dataCuts?.map((item) => ({
        value: item?.id?.toString(),
        label: isSearchable ? `${item.name}` : (item.name),
    }));
    const [selectedDataCut, setSelectedDataCut] = useState<Option | undefined>(() => ({
        value: dataCut?.toString(),
        label: dataCuts?.find(x => x.id === dataCut)?.name ?? 'Seleccione un corte',
    }));

    useEffect(() => {
        if (dataCut && dataCuts) {
            setSelectedDataCut({
                value: dataCut?.toString(),
                label: dataCuts?.find(x => x.id === dataCut)?.name ?? 'Seleccione un corte',
            });
        }
    }, [dataCut, setDataCut, dataCuts]);


    const handleChangeDataCut = (newValue: SingleValue<Option>) => {
        setSelectedDataCut({
            value: newValue?.value,
            label: newValue?.label,
        });
        setDataCut(Number(newValue?.value));
        if (xChange) {
            xChange(newValue);
        }
    }




    if (options)
        return (
            <Select
                name={name || 'idClient'}
                className={className}
                value={selectedDataCut}
                onChange={handleChangeDataCut}
                required={required}
                loadingMessage={() => 'Cargando...'}
                isDisabled={queryDataCuts?.isLoading || isDisabled}
                isLoading={queryDataCuts?.isLoading}
                options={options}
            />
        )
}