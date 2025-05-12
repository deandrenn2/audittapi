import { useEffect, useState } from "react";
import { EquivalenceSelect } from "../Settings/Scales/Equivalence/EquivalenceSelect"
import { ValuationModel } from "./AssessmentModel"
import { useAssessments } from "./useAssessment";
import Swal from "sweetalert2";

export const AssessmentValuations = ({ valuations, idScale, idAssessment, xSave }:
    {
        valuations: ValuationModel[] | undefined,
        idScale: number | undefined,
        idAssessment?: number,
        xSave?: (valuation: ValuationModel[]) => void
    }) => {

    const [values, setValues] = useState<ValuationModel[]>([]);
    const { deleteAssessment } = useAssessments();

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
            if (x.id === valuation.id) {
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

    function handleDelete(e: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro que deseas eliminar esta evaluación?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteAssessment.mutate(id);
            }
        })
    }

    return (
        <>
            <div className="bg-whitefont-semibold mb-4 flex gap-4">
                <h1 className=" text-2xl ">Evaluación de adherencia</h1>

                <button
                    className="bg-[#392F5A] border-2 transition-all hover:bg-purple-950 text-white  px-6 py-2 rounded-lg font-semibold cursor-pointer"
                    onClick={handleSave}
                >
                    Guardar
                </button>
                <button
                    className="border-[#FF677D] border-2 hover:bg-[#ff677e88] transition-all text-[#921729c4]  px-6 py-2 rounded-lg font-semibold cursor-pointer"
                    onClick={(e) => handleDelete(e, idAssessment ?? 0)}
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