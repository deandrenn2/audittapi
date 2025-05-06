import { useEffect, useState } from "react";
import { EquivalenceSelect } from "../Settings/Scales/Equivalence/EquivalenceSelect"
import { ValuationModel } from "./AssessmentModel"

export const AssessmentValuations = ({ valuations, idScale }: { valuations: ValuationModel[] | undefined, idScale: number | undefined }) => {

    const [values, setValues] = useState<ValuationModel[]>([]);

    useEffect(() => {
        if (!valuations) {
            return;
        }
        setValues(valuations ?? []);
    }, [valuations]);

    if (!valuations) {
        return <div>No se encontraron valoraciones</div>
    }

    const handleChange = (newValue: HTMLSelectElement, valuation: ValuationModel) => {
        const newValueNumber = values.map((x) => {
            if (x.idEquivalence === valuation.idEquivalence) {
                x.idEquivalence = Number(newValue.value);
                return x;
            }
            return x;
        })

        setValues(newValueNumber);
    }
    return (
        <div className="flex flex-col space-y-4">
            {values.map((valuation) => (
                <div key={valuation.id} className="w-full bg-green-100 border-2 border-green-200 rounded-2xl p-4 flex justify-between items-center">
                    <h2>{valuation.text}</h2>
                    <EquivalenceSelect idScale={idScale ?? 1} xChange={(x) => handleChange(x, valuation)} selectedValue={valuation.idEquivalence.toString()} name="idClient" isDisabled={false} />
                </div>
            ))}
        </div>
    )
}