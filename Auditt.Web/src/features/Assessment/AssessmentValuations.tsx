import { useEffect, useState } from "react";
import { EquivalenceSelect } from "../Settings/Scales/Equivalence/EquivalenceSelect"
import { ValuationModel } from "./AssessmentModel"

export const AssessmentValuations = ({ valuations, idScale, xSave }: { valuations: ValuationModel[] | undefined, idScale: number | undefined, xSave?: (valuation: ValuationModel[]) => void }) => {

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

    const handleSave = () => {
        if (xSave)
            xSave(values);
    }
    return (
        <>
            <div className="bg-whitefont-semibold mb-4 flex gap-4">
                <h1 className=" text-2xl ">Evaluación de adherencia</h1>
                <button
                    className="bg-[#392F5A] hover:bg-purple-950 text-white px-6 py-2 rounded-lg font-semibold cursor-pointer"
                >
                    Guardar y nuevo
                </button>

                <button
                    className="border-[#392F5A] border-2 hover:bg-purple-100 transition-all hover:border-purple-950 text-[#392F5A]  px-6 py-2 rounded-lg font-semibold cursor-pointer"
                    onClick={handleSave}
                >
                    Guardar
                </button>
                <button
                    className="bg-[#FF677D] hover:bg-[#ff677ec4] transition-all text-white px-6 py-2 rounded-lg font-semibold cursor-pointer"
                >
                    Eliminar
                </button>


                <button
                    className="border-[#392F5A] border-2 hover:bg-purple-100 transition-all hover:border-purple-950 text-[#392F5A]  px-6 py-2 rounded-lg font-semibold cursor-pointer"
                    onClick={handleSave}
                >
                    Agregar pregunta
                </button>
            </div>
            <div className="flex flex-col space-y-4">
                {values.map((valuation) => (
                    <div key={valuation.id} className="w-full bg-green-100 border-2 border-green-200 rounded-2xl p-4 flex justify-between items-center">
                        <h2>{valuation.text}</h2>
                        <EquivalenceSelect idScale={idScale ?? 1} xChange={(x) => handleChange(x, valuation)} selectedValue={valuation.idEquivalence.toString()} name="idClient" isDisabled={false} />
                    </div>
                ))}
            </div>
        </>
    )
}