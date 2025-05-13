import { SingleValue } from "react-select";
import { FunctionarySelect } from "../Clients/Professionals/FunctionarySelect"
import { useState } from "react";
import { Option } from "../../shared/model";
import { useReportsFunctionaryAdherence } from "./UseReports";

export const ReportFunctionaries = ({ dataCut, idGuide }: { dataCut: number, idGuide: number }) => {
    const [selectedFunctionary, setSelectedFunctionary] = useState<Option | undefined>(() => ({
        value: "0",
        label: "Seleccione un profesional",
    }));
    const { reportFunctionaryAdherence } = useReportsFunctionaryAdherence(dataCut, Number(selectedFunctionary?.value ?? "0"), idGuide);

    const handleChangeFunctionary = (newValue: SingleValue<Option>) => {
        setSelectedFunctionary({
            value: newValue?.value,
            label: newValue?.label,
        });
    }

    if (!selectedFunctionary || !dataCut || !idGuide) {
        return <div></div>;
    }

    return (
        <div className="w-full">
            <div className="flex py-2">
                <div className="flex align-middle items-center ">
                    <span className="font-medium">Profesional evaluado</span>
                    <FunctionarySelect className="w-full" selectedValue={selectedFunctionary} xChange={handleChangeFunctionary} isSearchable={true} />
                </div>
            </div>
            <div className="flex flex-col space-y-4">
                {reportFunctionaryAdherence?.map((valuation, index) => (
                    <div key={index} className="w-full mb-1 bg-green-100 border-2 border-green-200 rounded-2xl p-4 gap-2 flex justify-between items-center">
                        <h2>{valuation.text}</h2>
                        <h2 className="text-2xl font-bold text-audittpurple">{valuation.percentSuccess}%</h2>
                    </div>
                ))}
            </div>
        </div>
    )
}